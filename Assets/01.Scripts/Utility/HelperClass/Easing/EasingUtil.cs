using System.Reflection;
using UnityEngine;

public class Easing
{
	private EasingFunction Instance { get; }
	private MethodInfo[] Functions { get; }

	public static float Get(Ease type, float t)
	{
		var easing = new Easing();
		var interpolated = Mathf.Clamp(t, 0, 1);
		var method = easing.Functions[(int)type];
		return (float)method.Invoke(easing.Instance, new object[] { interpolated });
	}

	private Easing()
	{
		Instance = new EasingFunction();
		Functions = LoadEasingFunctionMethodInfo();
	}

	private MethodInfo[] LoadEasingFunctionMethodInfo()
	{
		var type = Instance.GetType();
		return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
	}
}

public class EasingFunction
{
	public float Linear(float t) => t;

	public float InQuad(float t) => t * t;
	public float OutQuad(float t) => 1 - InQuad(1 - t);

	public float InOutQuad(float t)
	{
		if (t < 0.5) return InQuad(t * 2) / 2;
		return 1 - InQuad((1 - t) * 2) / 2;
	}

	public float InCubic(float t) => t * t * t;
	public float OutCubic(float t) => 1 - InCubic(1 - t);

	public float InOutCubic(float t)
	{
		if (t < 0.5) return InCubic(t * 2) / 2;
		return 1 - InCubic((1 - t) * 2) / 2;
	}

	public float InQuart(float t) => t * t * t * t;
	public float OutQuart(float t) => 1 - InQuart(1 - t);

	public float InOutQuart(float t)
	{
		if (t < 0.5) return InQuart(t * 2) / 2;
		return 1 - InQuart((1 - t) * 2) / 2;
	}

	public float InQuint(float t) => t * t * t * t * t;
	public float OutQuint(float t) => 1 - InQuint(1 - t);

	public float InOutQuint(float t)
	{
		if (t < 0.5) return InQuint(t * 2) / 2;
		return 1 - InQuint((1 - t) * 2) / 2;
	}

	public float InSine(float t) => -Mathf.Cos(t * Mathf.PI / 2);
	public float OutSine(float t) => Mathf.Sin(t * Mathf.PI / 2);
	public float InOutSine(float t) => (Mathf.Cos(t * Mathf.PI) - 1) / -2;

	public float InExpo(float t) => Mathf.Pow(2, 10 * (t - 1));
	public float OutExpo(float t) => 1 - InExpo(1 - t);

	public float InOutExpo(float t)
	{
		if (t < 0.5) return InExpo(t * 2) / 2;
		return 1 - InExpo((1 - t) * 2) / 2;
	}

	public float InCirc(float t) => -(Mathf.Sqrt(1 - t * t) - 1);
	public float OutCirc(float t) => 1 - InCirc(1 - t);

	public float InOutCirc(float t)
	{
		if (t < 0.5) return InCirc(t * 2) / 2;
		return 1 - InCirc((1 - t) * 2) / 2;
	}

	public float InElastic(float t) => 1 - OutElastic(1 - t);

	public float OutElastic(float t)
	{
		float p = 0.3f;
		return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4) * (2 * Mathf.PI) / p) + 1;
	}

	public float InOutElastic(float t)
	{
		if (t < 0.5) return InElastic(t * 2) / 2;
		return 1 - InElastic((1 - t) * 2) / 2;
	}

	public float InBack(float t)
	{
		float s = 1.70158f;
		return t * t * ((s + 1) * t - s);
	}

	public float OutBack(float t) => 1 - InBack(1 - t);

	public float InOutBack(float t)
	{
		if (t < 0.5) return InBack(t * 2) / 2;
		return 1 - InBack((1 - t) * 2) / 2;
	}

	public float InBounce(float t) => 1 - OutBounce(1 - t);

	public float OutBounce(float t)
	{
		float div = 2.75f;
		float mult = 7.5625f;

		if (t < 1 / div)
		{
			return mult * t * t;
		}
		else if (t < 2 / div)
		{
			t -= 1.5f / div;
			return mult * t * t + 0.75f;
		}
		else if (t < 2.5 / div)
		{
			t -= 2.25f / div;
			return mult * t * t + 0.9375f;
		}
		else
		{
			t -= 2.625f / div;
			return mult * t * t + 0.984375f;
		}
	}

	public float InOutBounce(float t)
	{
		if (t < 0.5) return InBounce(t * 2) / 2;
		return 1 - InBounce((1 - t) * 2) / 2;
	}
}
