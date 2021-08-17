/* 
    Credits/References:
    https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/?ref=hackernoon.com (Robert Utter)
    https://hackernoon.com/lerping-with-coroutines-and-animation-curves-4185b30f6002 (@rhysp, Rhys Patterson)
    https://medium.com/@rhysp/lerping-with-coroutines-and-animation-curves-4185b30f6002 (@rhysp, Rhys Patterson)
    https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/#lerp_with_easing
    http://dotween.demigiant.com/documentation.php  
*/

using UnityEngine;
using System.Collections;

namespace ƒx.UnityUtils
{

    public class Transitions
    {
        /*
            Mathf.Lerp
            Mathf.LerpUnclamped
            Mathf.InverseLerp
            Mathf.LerpAngle
            Mathf.SmoothDamp
            Mathf.SmoothDampAngle
            Mathf.MoveTowards
            Mathf.MoveTowardsAngle

            Vector3.Lerp
            Vector3.LerpUnclamped
            Vector3.SmoothDamp
            Vector3.Slerp
            Vector3.SlerpUnclamped

            Quaternion.Lerp
            ...

            Color.Lerp
            Color.LerpUnclamped

            DOTween.To(         ()=> myFloat  ,   x=> myFloat = x  ,   52  ,         1.5f      );
            static DOTween.To(     getter     ,      setter        ,   to  ,   float duration  );
        */

        /* GENERIC Interpolation ??? ---------------------------------------*/
        // TODO: interface (type of object (vetor, float, color...), type of interpolation (linear, smoothstep...)
        public delegate object GetObject();
        public delegate void SetObject(object o);
        public delegate object Interpolation(object startValue, object endValue, float t);
        public delegate void Interpolate(GetObject getter, SetObject setter, object endValue, float duration, Interpolation interpolation);
        /*-----------------------------------------------------------------*/


        /* Vector3 Lerp ---------------------------------------------------*/
        public delegate Vector3 GetVector3();
        public delegate void SetVector3(Vector3 v);

        static public IEnumerator LerpVector3(GetVector3 getter, SetVector3 setter, Vector3 endValue, float duration)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            Vector3 startValue = getter();

            while (elapsedTime < duration)
            {
                Vector3 newValue = Vector3.Lerp(startValue, endValue, elapsedTime / duration);
                setter(newValue);
                elapsedTime = Time.time - startTime;
                yield return null;
            }
            setter(endValue);
            yield break;
        }
        /*-----------------------------------------------------------------*/


        /* Color Lerp -----------------------------------------------------*/
        public delegate Color GetColor();
        public delegate void SetColor(Color c);

        static public IEnumerator LerpColor(GetColor getter, SetColor setter, Color endValue, float duration)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            Color startValue = getter();

            while (elapsedTime < duration)
            {
                Color newValue = Color.Lerp(startValue, endValue, elapsedTime / duration);
                setter(newValue);
                elapsedTime = Time.time - startTime;
                yield return null;
            }
            setter(endValue);
            yield break;
        }
        /*-----------------------------------------------------------------*/

         /* Float Lerp -----------------------------------------------------*/
        public delegate float GetFloat();
        public delegate void SetFloat(float f);

        static public IEnumerator LerpFloat(GetFloat getter, SetFloat setter, float endValue, float duration)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            float startValue = getter();

            while (elapsedTime < duration)
            {
                float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                setter(newValue);
                elapsedTime = Time.time - startTime;
                yield return null;
            }
            setter(endValue);
            yield break;
        }
        /*-----------------------------------------------------------------*/

        // TODO: implement different interpolation functions
        float SmoothStep(float t)
        { return t * t * (3f - 2f * t); }

        float SmootherStep(float t)
        { return t * t * t * (t * (6f * t - 15f) + 10f); }

        float Quadratic(float t)
        { return t * t; }

        float Coserp(float t) // ease in
        { return 1f - Mathf.Cos(t * Mathf.PI * 0.5f); }

        float Sinerp(float t) // ease out
        { return t = Mathf.Sin(t * Mathf.PI * 0.5f); }

        float Crverp(float t, AnimationCurve crv)
        { return crv.Evaluate(t); }


        public AnimationCurve GetLinearCurve()
        {
            return new AnimationCurve(
                new Keyframe(0, 0, 1, 1),
                new Keyframe(1, 1, 1, 1)
                );
        }

        public AnimationCurve GetEasingInOutCurve()
        {
            return new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(1, 1)
                );
        }
    }
}