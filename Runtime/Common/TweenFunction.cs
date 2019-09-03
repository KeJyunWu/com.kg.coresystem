using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.UI;

public class TweenFunction : MonoBehaviour
{

    static public void TweenRectTransformPos(RectTransform _Obj, Vector3 _TargetPos, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));

        sequence.Insert(_delay, HOTween.To(_Obj, _TweenTime, new TweenParms().Prop("localPosition", _TargetPos).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenRectTransformSize(RectTransform _Obj, Vector2 _TargetScale, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));

        sequence.Insert(_delay, HOTween.To(_Obj, _TweenTime, new TweenParms().Prop("sizeDelta", _TargetScale).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenRectTransformScale(RectTransform _Obj, Vector3 _TargetScale, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));

        sequence.Insert(_delay, HOTween.To(_Obj, _TweenTime, new TweenParms().Prop("localScale", _TargetScale).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenImgColor(Image _img, Color _Color, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));
        sequence.Insert(_delay, HOTween.To(_img, _TweenTime, new TweenParms().Prop("color", _Color).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenRawImgColor(RawImage _img, Color _Color, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));
        sequence.Insert(_delay, HOTween.To(_img, _TweenTime, new TweenParms().Prop("color", _Color).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenTextColor(Text _img, Color _Color, EaseType _TweenType, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));
        sequence.Insert(_delay, HOTween.To(_img, _TweenTime, new TweenParms().Prop("color", _Color).Ease(_TweenType)));
        sequence.Play();
    }

    static public void TweenCanvasGroupAlpha(CanvasGroup _canvasGroup, float _alpha, float _TweenTime, Holoville.HOTween.Core.TweenDelegate.TweenCallback _Callback = null, float _delay = 0.0f, EaseType _TweenType = EaseType.Linear)
    {
        Sequence sequence;
        if (_Callback != null)
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo).OnComplete(_Callback));
        else
            sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));

        sequence.Insert(_delay, HOTween.To(_canvasGroup, _TweenTime, new TweenParms().Prop("alpha", _alpha).Ease(_TweenType)));
        sequence.Play();
    }
}
