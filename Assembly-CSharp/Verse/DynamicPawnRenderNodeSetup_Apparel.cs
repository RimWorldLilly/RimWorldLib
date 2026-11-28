using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A0 RID: 928
	public class DynamicPawnRenderNodeSetup_Apparel : DynamicPawnRenderNodeSetup
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x000AE297 File Offset: 0x000AC497
		public override bool HumanlikeOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000AE29A File Offset: 0x000AC49A
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		public override IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
		{
			DynamicPawnRenderNodeSetup_Apparel.<GetDynamicNodes>d__3 <GetDynamicNodes>d__ = new DynamicPawnRenderNodeSetup_Apparel.<GetDynamicNodes>d__3(-2);
			<GetDynamicNodes>d__.<>3__pawn = pawn;
			<GetDynamicNodes>d__.<>3__tree = tree;
			return <GetDynamicNodes>d__;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000AE2B1 File Offset: 0x000AC4B1
		private static bool ShouldAddApparelNode(Apparel gear)
		{
			return !gear.def.IsWeapon;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000AE2C1 File Offset: 0x000AC4C1
		[return: TupleElementNames(new string[]
		{
			"node",
			"parent"
		})]
		private static IEnumerable<ValueTuple<PawnRenderNode, PawnRenderNode>> ProcessApparel(Pawn pawn, PawnRenderTree tree, Apparel ap, PawnRenderNode headApparelNode, PawnRenderNode bodyApparelNode, Dictionary<PawnRenderNode, int> layerOffsets)
		{
			DynamicPawnRenderNodeSetup_Apparel.<ProcessApparel>d__5 <ProcessApparel>d__ = new DynamicPawnRenderNodeSetup_Apparel.<ProcessApparel>d__5(-2);
			<ProcessApparel>d__.<>3__pawn = pawn;
			<ProcessApparel>d__.<>3__tree = tree;
			<ProcessApparel>d__.<>3__ap = ap;
			<ProcessApparel>d__.<>3__headApparelNode = headApparelNode;
			<ProcessApparel>d__.<>3__bodyApparelNode = bodyApparelNode;
			<ProcessApparel>d__.<>3__layerOffsets = layerOffsets;
			return <ProcessApparel>d__;
		}

		// Token: 0x04001703 RID: 5891
		private const int ApparelLayerShellNorth = 88;
	}
}
