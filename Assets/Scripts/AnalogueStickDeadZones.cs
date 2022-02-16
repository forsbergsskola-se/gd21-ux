using UnityEngine;

public class AnalogueStickDeadZones : MonoBehaviour
{
    [SerializeField] private float radialDeadZoneLow;
    [SerializeField] private float radialDeadZoneHigh;
    [SerializeField] private float angularDeadZone;

    [SerializeField] private Transform unprocessedVisualizer;
    [SerializeField] private Transform radialDeadZoneVisualizer;
    [SerializeField] private Transform angularDeadZoneVisualizer;

    private void Update()
    {
        //Get our input.
        var unprocessedAnalogueStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var radialDeadZoneProcessedInput = RadialDeadZoneProcessing(unprocessedAnalogueStickInput);
        var angularDeadZoneProcessedInput = AngularDeadZoneProcessing(radialDeadZoneProcessedInput);

        //Apply input to visualization objects.
        unprocessedVisualizer.transform.localPosition = unprocessedAnalogueStickInput;
        radialDeadZoneVisualizer.transform.localPosition = radialDeadZoneProcessedInput;
        angularDeadZoneVisualizer.transform.localPosition = angularDeadZoneProcessedInput;
    }

    private Vector2 RadialDeadZoneProcessing(Vector2 data)
    {
        var magnitude = data.magnitude;
        if (magnitude < radialDeadZoneLow)
            return Vector2.zero;

        var legalRange = 1f - radialDeadZoneHigh - radialDeadZoneLow;
        var normalizedMagnitude = Mathf.Min(1f, (magnitude - radialDeadZoneLow) / legalRange);
        var scale = normalizedMagnitude / magnitude;
        return data * scale;
    }

    private Vector2 AngularDeadZoneProcessing(Vector2 data)
    {
        var magnitude = data.magnitude;
        var angle = Vector2.Angle(data, Vector2.right); //Start counting from the right in a unit circle.
        if (data.y < 0)
        {
            angle = 360f - angle;
        }

        var angleModulo = angle % 90;
        var quadrant = Mathf.FloorToInt(angle / 90);

        if (angleModulo < angularDeadZone)
        {
            angle = quadrant * 90 * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
        }

        if (angleModulo > 90 - angularDeadZone)
        {
            angle = (quadrant + 1) * 90 * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
        }

        var legalRange = 90 - angularDeadZone * 2;
        var scaledAngle = (90 * (angle - angularDeadZone * (quadrant * 2 + 1)) / legalRange) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(scaledAngle), Mathf.Sin(scaledAngle)) * magnitude;
    }
}