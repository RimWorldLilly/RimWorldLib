using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200002D RID: 45
	public class ComplexCurve : IEquatable<ComplexCurve>
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00009CC3 File Offset: 0x00007EC3
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00009CD0 File Offset: 0x00007ED0
		public UnityEngine.Keyframe[] Keys
		{
			get
			{
				return this.curve.keys;
			}
			set
			{
				this.curve.keys = value;
				this.keyframes = value.ToList<UnityEngine.Keyframe>();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00009CEA File Offset: 0x00007EEA
		public int Length
		{
			get
			{
				return this.curve.length;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00009CF7 File Offset: 0x00007EF7
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00009D04 File Offset: 0x00007F04
		public WrapMode PreWrapMode
		{
			get
			{
				return this.curve.preWrapMode;
			}
			set
			{
				this.curve.preWrapMode = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00009D12 File Offset: 0x00007F12
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00009D1F File Offset: 0x00007F1F
		public WrapMode PostWrapMode
		{
			get
			{
				return this.curve.postWrapMode;
			}
			set
			{
				this.curve.postWrapMode = value;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009D2D File Offset: 0x00007F2D
		public ComplexCurve()
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00009D43 File Offset: 0x00007F43
		public ComplexCurve(params UnityEngine.Keyframe[] keyframes)
		{
			this.curve = new AnimationCurve(keyframes);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009D65 File Offset: 0x00007F65
		public ComplexCurve(AnimationCurve curve)
		{
			this.curve = curve;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009D82 File Offset: 0x00007F82
		public float Evaluate(float time)
		{
			return this.curve.Evaluate(time);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009D90 File Offset: 0x00007F90
		public int AddKey(float time, float value)
		{
			return this.curve.AddKey(time, value);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009D9F File Offset: 0x00007F9F
		public int AddKey(UnityEngine.Keyframe keyframe)
		{
			return this.curve.AddKey(keyframe);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009DAD File Offset: 0x00007FAD
		public int MoveKey(int index, UnityEngine.Keyframe keyframe)
		{
			return this.curve.MoveKey(index, keyframe);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009DBC File Offset: 0x00007FBC
		public void RemoveKey(int index)
		{
			this.curve.RemoveKey(index);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009DCA File Offset: 0x00007FCA
		public void SmoothTangents(int index, float weight)
		{
			this.curve.SmoothTangents(index, weight);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00009DD9 File Offset: 0x00007FD9
		public static ComplexCurve Constant(float timeStart, float timeEnd, float value)
		{
			return new ComplexCurve(AnimationCurve.Constant(timeStart, timeEnd, value));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public static ComplexCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new ComplexCurve(AnimationCurve.Linear(timeStart, valueStart, timeEnd, valueEnd));
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00009DF8 File Offset: 0x00007FF8
		public static ComplexCurve LinearNormalized
		{
			get
			{
				return new ComplexCurve(AnimationCurve.Linear(0f, 0f, 1f, 1f));
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009E18 File Offset: 0x00008018
		public static ComplexCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return new ComplexCurve(AnimationCurve.EaseInOut(timeStart, valueStart, timeEnd, valueEnd));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009E28 File Offset: 0x00008028
		public static ComplexCurve Empty()
		{
			return new ComplexCurve(new AnimationCurve());
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00009E34 File Offset: 0x00008034
		public AnimationCurve GetInternalCurveCopy()
		{
			return new AnimationCurve(this.curve.keys)
			{
				preWrapMode = this.curve.preWrapMode,
				postWrapMode = this.curve.postWrapMode
			};
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00009E68 File Offset: 0x00008068
		public void PostLoad()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.preWrapMode = this.preWrapMode;
			animationCurve.postWrapMode = this.postWrapMode;
			List<UnityEngine.Keyframe> list = this.keyframes;
			animationCurve.keys = ((list != null) ? list.ToArray() : null);
			this.curve = animationCurve;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00009EA5 File Offset: 0x000080A5
		public override bool Equals(object o)
		{
			return o != null && (this == o || (o.GetType() == base.GetType() && this.Equals((ComplexCurve)o)));
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009ED3 File Offset: 0x000080D3
		public bool Equals(ComplexCurve other)
		{
			return other != null && this.curve.Equals(other.curve);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009EEB File Offset: 0x000080EB
		public override int GetHashCode()
		{
			return this.curve.GetHashCode();
		}

		// Token: 0x0400005F RID: 95
		private List<UnityEngine.Keyframe> keyframes;

		// Token: 0x04000060 RID: 96
		private WrapMode preWrapMode = WrapMode.ClampForever;

		// Token: 0x04000061 RID: 97
		private WrapMode postWrapMode = WrapMode.ClampForever;

		// Token: 0x04000062 RID: 98
		[Unsaved(false)]
		private AnimationCurve curve;
	}
}
