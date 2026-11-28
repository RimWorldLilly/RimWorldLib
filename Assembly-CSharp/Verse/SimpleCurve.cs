using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000072 RID: 114
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable, IExposable
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00019277 File Offset: 0x00017477
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00019284 File Offset: 0x00017484
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001928C File Offset: 0x0001748C
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00019297 File Offset: 0x00017497
		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x000192BE File Offset: 0x000174BE
		public float MinY
		{
			get
			{
				return this.points.Min((CurvePoint point) => point.y);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x000192EA File Offset: 0x000174EA
		public float MaxY
		{
			get
			{
				return this.points.Max((CurvePoint point) => point.y);
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00019316 File Offset: 0x00017516
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00019330 File Offset: 0x00017530
		public SimpleCurve()
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00019343 File Offset: 0x00017543
		public void ExposeData()
		{
			Scribe_Collections.Look<CurvePoint>(ref this.points, "points", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001935B File Offset: 0x0001755B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00019363 File Offset: 0x00017563
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			SimpleCurve.<GetEnumerator>d__19 <GetEnumerator>d__ = new SimpleCurve.<GetEnumerator>d__19(0);
			<GetEnumerator>d__.<>4__this = this;
			return <GetEnumerator>d__;
		}

		// Token: 0x170000D9 RID: 217
		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00019390 File Offset: 0x00017590
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000193F0 File Offset: 0x000175F0
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001940E File Offset: 0x0001760E
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00019425 File Offset: 0x00017625
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00019438 File Offset: 0x00017638
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.");
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00019498 File Offset: 0x00017698
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000194F8 File Offset: 0x000176F8
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int i = 0;
			while (i < this.points.Count)
			{
				if (x <= this.points[i].x)
				{
					curvePoint2 = this.points[i];
					if (i > 0)
					{
						curvePoint = this.points[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00019640 File Offset: 0x00017840
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (this.points.Count == 1)
			{
				return this.points[0].x;
			}
			int i = 0;
			while (i < this.points.Count - 1)
			{
				if ((y >= this.points[i].y && y <= this.points[i + 1].y) || (y <= this.points[i].y && y >= this.points[i + 1].y))
				{
					if (y == this.points[i].y)
					{
						return this.points[i].x;
					}
					if (y == this.points[i + 1].y)
					{
						return this.points[i + 1].x;
					}
					return GenMath.LerpDouble(this.points[i].y, this.points[i + 1].y, this.points[i].x, this.points[i + 1].x, y);
				}
				else
				{
					i++;
				}
			}
			if (y < this.points[0].y)
			{
				float result = 0f;
				float num = 0f;
				for (int j = 0; j < this.points.Count; j++)
				{
					if (j == 0 || this.points[j].y < num)
					{
						num = this.points[j].y;
						result = this.points[j].x;
					}
				}
				return result;
			}
			float result2 = 0f;
			float num2 = 0f;
			for (int k = 0; k < this.points.Count; k++)
			{
				if (k == 0 || this.points[k].y > num2)
				{
					num2 = this.points[k].y;
					result2 = this.points[k].x;
				}
			}
			return result2;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000198C4 File Offset: 0x00017AC4
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + ((this != null) ? this.ToString() : null) + ": slope should never be negative.");
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001995A File Offset: 0x00017B5A
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			SimpleCurve.<ConfigErrors>d__32 <ConfigErrors>d__ = new SimpleCurve.<ConfigErrors>d__32(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__prefix = prefix;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00019971 File Offset: 0x00017B71
		public static SimpleCurve Empty()
		{
			return new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				}
			};
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00019990 File Offset: 0x00017B90
		public AnimationCurve ToAnimationCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			foreach (CurvePoint curvePoint in this.Points)
			{
				animationCurve.AddKey(curvePoint.x, curvePoint.y);
			}
			return animationCurve;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000199F8 File Offset: 0x00017BF8
		public static SimpleCurve FromAnimationCurve(AnimationCurve curve)
		{
			SimpleCurve simpleCurve = new SimpleCurve();
			foreach (Keyframe keyframe in curve.keys)
			{
				simpleCurve.Points.Add(new CurvePoint(keyframe.time, keyframe.value));
			}
			return simpleCurve;
		}

		// Token: 0x040001A3 RID: 419
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x040001A4 RID: 420
		[Unsaved(false)]
		private SimpleCurveView view;

		// Token: 0x040001A5 RID: 421
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};
	}
}
