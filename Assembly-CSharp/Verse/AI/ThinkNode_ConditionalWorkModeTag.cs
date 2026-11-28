using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008CC RID: 2252
	public class ThinkNode_ConditionalWorkModeTag : ThinkNode_Conditional
	{
		// Token: 0x060045BE RID: 17854 RVA: 0x001B7544 File Offset: 0x001B5744
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.RaceProps.IsMechanoid && pawn.Faction == Faction.OfPlayer && pawn.relations != null)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
				return ((firstDirectRelationPawn != null) ? firstDirectRelationPawn.mechanitor.GetControlGroup(pawn).GetTag(pawn) : null) == this.tag;
			}
			return false;
		}
	}
}
