using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001825 RID: 6181
	public class AttachPointTracker
	{
		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x06009A98 RID: 39576 RVA: 0x0037C2F3 File Offset: 0x0037A4F3
		public string ThingId
		{
			get
			{
				return this.parent.ThingID;
			}
		}

		// Token: 0x06009A99 RID: 39577 RVA: 0x0037C300 File Offset: 0x0037A500
		public AttachPointTracker(List<AttachPoint> points, ThingWithComps parent)
		{
			this.parent = parent;
			this.cache = new Dictionary<AttachPointType, AttachPoint>();
			foreach (AttachPoint attachPoint in points)
			{
				this.cache[attachPoint.type] = attachPoint;
			}
			this.cache[AttachPointType.RootNone] = new AttachPoint
			{
				offset = Vector3.zero,
				type = AttachPointType.RootNone
			};
		}

		// Token: 0x06009A9A RID: 39578 RVA: 0x0037C394 File Offset: 0x0037A594
		public void Add(AttachPointTracker other, bool overwrite = true)
		{
			foreach (KeyValuePair<AttachPointType, AttachPoint> keyValuePair in other.cache)
			{
				if (overwrite || !this.cache.ContainsKey(keyValuePair.Key))
				{
					this.cache[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06009A9B RID: 39579 RVA: 0x0037C410 File Offset: 0x0037A610
		private void TryCacheKindDefOverrides()
		{
			if (!this.didReadOverrides)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					AlternateGraphic alternateGraphic;
					int num;
					if (pawn.TryGetAlternate(out alternateGraphic, out num) && alternateGraphic.attachPoints != null)
					{
						foreach (AttachPoint attachPoint in alternateGraphic.attachPoints)
						{
							this.cache[attachPoint.type] = attachPoint;
						}
					}
					Graphic bodyGraphic = pawn.Drawer.renderer.BodyGraphic;
					GraphicData graphicData = (bodyGraphic != null) ? bodyGraphic.data : null;
					if (graphicData == null)
					{
						return;
					}
					if (graphicData.attachPoints != null && graphicData.attachPoints.Count > 0)
					{
						foreach (AttachPoint attachPoint2 in graphicData.attachPoints)
						{
							this.cache[attachPoint2.type] = attachPoint2;
						}
					}
					this.didReadOverrides = true;
				}
			}
		}

		// Token: 0x06009A9C RID: 39580 RVA: 0x0037C534 File Offset: 0x0037A734
		public Vector3 GetRotatedOffset(AttachPointType type)
		{
			return this.GetRotatedOffset(type, this.parent.Rotation);
		}

		// Token: 0x06009A9D RID: 39581 RVA: 0x0037C548 File Offset: 0x0037A748
		public Vector3 GetRotatedOffset(AttachPointType type, Rot4 rot)
		{
			Vector3 vector = this.GetPointOrDefault(type).offset;
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.attachPointScaleFactor;
			}
			return vector.RotatedBy(rot);
		}

		// Token: 0x06009A9E RID: 39582 RVA: 0x0037C58F File Offset: 0x0037A78F
		public Vector3 GetWorldPos(AttachPointType type)
		{
			return this.parent.DrawPos + this.GetRotatedOffset(type);
		}

		// Token: 0x06009A9F RID: 39583 RVA: 0x0037C5A8 File Offset: 0x0037A7A8
		public IEnumerable<AttachPointType> PointTypes(int min = 0, int max = 2147483647)
		{
			AttachPointTracker.<PointTypes>d__11 <PointTypes>d__ = new AttachPointTracker.<PointTypes>d__11(-2);
			<PointTypes>d__.<>4__this = this;
			<PointTypes>d__.<>3__min = min;
			<PointTypes>d__.<>3__max = max;
			return <PointTypes>d__;
		}

		// Token: 0x06009AA0 RID: 39584 RVA: 0x0037C5C6 File Offset: 0x0037A7C6
		public IEnumerable<AttachPoint> Points(int min = 0, int max = 2147483647)
		{
			AttachPointTracker.<Points>d__12 <Points>d__ = new AttachPointTracker.<Points>d__12(-2);
			<Points>d__.<>4__this = this;
			<Points>d__.<>3__min = min;
			<Points>d__.<>3__max = max;
			return <Points>d__;
		}

		// Token: 0x06009AA1 RID: 39585 RVA: 0x0037C5E4 File Offset: 0x0037A7E4
		public AttachPoint GetPointOrDefault(AttachPointType type)
		{
			AttachPoint result;
			if (this.TryGetPoint(type, out result))
			{
				return result;
			}
			return this.cache[AttachPointType.RootNone];
		}

		// Token: 0x06009AA2 RID: 39586 RVA: 0x0037C60A File Offset: 0x0037A80A
		public bool TryGetPoint(AttachPointType type, out AttachPoint pt)
		{
			return this.cache.TryGetValue(type, out pt);
		}

		// Token: 0x04005BE9 RID: 23529
		private ThingWithComps parent;

		// Token: 0x04005BEA RID: 23530
		private Dictionary<AttachPointType, AttachPoint> cache;

		// Token: 0x04005BEB RID: 23531
		private bool didReadOverrides;
	}
}
