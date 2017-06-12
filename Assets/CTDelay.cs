
using System;


public class CTDelay
{

    CTIntegrator sum;

    public float K;

    float lastValue = 0;

    public CTDelay()
    {

    }

    public CTDelay(float startCondition, float K)
    {
        sum = new CTIntegrator(startCondition);
        this.K = K;
    }

    public float Gain
    {
        get
        {
            return K;
        }
    }

    public float Next(float x, float deltaT)
    {
        x -= lastValue * K;
        x = sum.Next(x * deltaT);
        lastValue = x;
        return x * K;
    }
}

