using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
namespace BlocksGame
{

    public static class AnimationExtensions
    {
        public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Func<double, Color> transform = (t) =>
                Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                               fromColor.Green + t * (toColor.Green - fromColor.Green),
                               fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                               fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));

            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static void CancelAnimation(this VisualElement self) => self.AbortAnimation("ColorTo");

        static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate(name, new Animation((d) =>
            {
                var color = transform(d);
                callback(color);
            }),
            length: length,
            easing: easing,
            finished: (d, b) => taskCompletionSource.SetResult(b));

            return taskCompletionSource.Task;
        }
    }

}

