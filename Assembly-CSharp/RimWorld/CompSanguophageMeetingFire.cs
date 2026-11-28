using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001961 RID: 6497
	[StaticConstructorOnStartup]
	public class CompSanguophageMeetingFire : CompFireOverlayBase
	{
		// Token: 0x17001C3E RID: 7230
		// (get) Token: 0x0600A311 RID: 41745 RVA: 0x003A3FA0 File Offset: 0x003A21A0
		public new CompProperties_SanguophageMeetingFire Props
		{
			get
			{
				return (CompProperties_SanguophageMeetingFire)this.props;
			}
		}

		// Token: 0x0600A312 RID: 41746 RVA: 0x003A3FB0 File Offset: 0x003A21B0
		public override void PostDraw()
		{
			base.PostDraw();
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null && !compGlower.Glows)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompSanguophageMeetingFire.RedlightGraphic.Draw(drawPos + this.Props.offset, Rot4.North, this.parent, 0f);
		}

		// Token: 0x0600A313 RID: 41747 RVA: 0x003A4027 File Offset: 0x003A2227
		public override bool CompPreventClaimingBy(Faction faction)
		{
			Lord lord = ((Building)this.parent).GetLord();
			return ((lord != null) ? lord.CurLordToil : null) is LordToil_SanguophageMeeting;
		}

		// Token: 0x04006055 RID: 24661
		public static readonly Graphic RedlightGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Redlight", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
