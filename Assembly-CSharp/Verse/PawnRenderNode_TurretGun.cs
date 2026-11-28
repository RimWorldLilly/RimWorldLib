using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B1 RID: 945
	public class PawnRenderNode_TurretGun : PawnRenderNode
	{
		// Token: 0x06001C51 RID: 7249 RVA: 0x000AEAFA File Offset: 0x000ACCFA
		public PawnRenderNode_TurretGun(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000AEB05 File Offset: 0x000ACD05
		public override Graphic GraphicFor(Pawn pawn)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.turretComp.Props.turretDef.graphicData.texPath, ShaderDatabase.Cutout);
		}

		// Token: 0x04001707 RID: 5895
		public CompTurretGun turretComp;
	}
}
