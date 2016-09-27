using UnityEngine;

public static class __VisualEffects
{
    public static System.Collections.IEnumerator Blink(SpriteRenderer renderer, float duration, float rate, Color normal_color)
    {
        int count = (int)(duration / rate);

        Color new_color;

        float red = 1.0f;
        float green = 0.0f;
        float blue = 0.0f;
        float alpha = 1.0f;

        float delta_red = 0.5f;
        float delta_green = 0.5f;
        float delta_blue = 0.0f;
        float delta_alpha = 0.5f;

        for (int i = 1; i < count; i++)
        {
            delta_red *= -1;
            red += delta_red;

            delta_green *= -1;
            green += delta_green;

            delta_blue *= -1;
            blue += delta_blue;

            delta_alpha *= -1;
            alpha += delta_alpha;

            new_color = new Color(
                red,
                green,
                blue,
                alpha
                );

            renderer.color = new_color;
            yield return new WaitForSeconds(rate);
        }

        renderer.color = normal_color;
    }

    public static System.Collections.IEnumerator Transparency(SpriteRenderer renderer, float duration, float alpha)
    {
        Color old_color = renderer.color;

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);

        yield return new WaitForSeconds(duration);

        renderer.color = old_color;
    }
}
