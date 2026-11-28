using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000073 RID: 115
	public class SimpleCurveView
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00019A5E File Offset: 0x00017C5E
		public IEnumerable<float> DebugInputValues
		{
			get
			{
				SimpleCurveView.<get_DebugInputValues>d__5 <get_DebugInputValues>d__ = new SimpleCurveView.<get_DebugInputValues>d__5(-2);
				<get_DebugInputValues>d__.<>4__this = this;
				return <get_DebugInputValues>d__;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00019A6E File Offset: 0x00017C6E
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00019A7D File Offset: 0x00017C7D
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00019A9C File Offset: 0x00017C9C
		public void SetViewRectAround(SimpleCurve curve)
		{
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
				return;
			}
			this.rect.xMin = (from pt in curve.Points
			select pt.Loc.x).Min();
			this.rect.xMax = (from pt in curve.Points
			select pt.Loc.x).Max();
			this.rect.yMin = (from pt in curve.Points
			select pt.Loc.y).Min();
			this.rect.yMax = (from pt in curve.Points
			select pt.Loc.y).Max();
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = this.rect.xMin * 2f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = this.rect.yMin * 2f;
			}
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = 1f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = 1f;
			}
			float width = this.rect.width;
			float height = this.rect.height;
			this.rect.xMin = this.rect.xMin - width * 0.1f;
			this.rect.xMax = this.rect.xMax + width * 0.1f;
			this.rect.yMin = this.rect.yMin - height * 0.1f;
			this.rect.yMax = this.rect.yMax + height * 0.1f;
		}

		// Token: 0x040001A6 RID: 422
		public Rect rect;

		// Token: 0x040001A7 RID: 423
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x040001A8 RID: 424
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x040001A9 RID: 425
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}
