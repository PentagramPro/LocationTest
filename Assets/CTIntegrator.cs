using System;

public class CTIntegrator
{
    float value = 0;

    public CTIntegrator()
    {

    }

    public CTIntegrator(float initialCondition)
    {
        value = initialCondition;
    }

    public float Next(float input)
    {
        value += input;
        return value;
    }
}

