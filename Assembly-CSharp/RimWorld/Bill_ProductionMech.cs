using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD9 RID: 6873
	public class Bill_ProductionMech : Bill_Mech
	{
		// Token: 0x17001F9B RID: 8091
		// (get) Token: 0x0600AF5D RID: 44893 RVA: 0x003DBD80 File Offset: 0x003D9F80
		public override float BandwidthCost
		{
			get
			{
				return this.recipe.ProducedThingDef.GetStatValueAbstract(StatDefOf.BandwidthCost, null);
			}
		}

		// Token: 0x0600AF5E RID: 44894 RVA: 0x003DBD98 File Offset: 0x003D9F98
		public Bill_ProductionMech()
		{
		}

		// Token: 0x0600AF5F RID: 44895 RVA: 0x003DBDA0 File Offset: 0x003D9FA0
		public Bill_ProductionMech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF60 RID: 44896 RVA: 0x003DBDAC File Offset: 0x003D9FAC
		public override Thing CreateProducts()
		{
			Pawn boundPawn = base.BoundPawn;
			PawnKindDef kind = (from pk in DefDatabase<PawnKindDef>.AllDefs
			where pk.race == this.recipe.ProducedThingDef
			select pk).First<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, boundPawn.Faction, PawnGenerationContext.NonPlayer, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false));
			boundPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, pawn);
			return pawn;
		}

		// Token: 0x0600AF61 RID: 44897 RVA: 0x003DBE88 File Offset: 0x003DA088
		public override void AppendInspectionData(StringBuilder sb)
		{
			switch (base.State)
			{
			case FormingState.Gathering:
				base.AppendCurrentIngredientCount(sb);
				break;
			case FormingState.Preparing:
				sb.AppendLine("GestatingMech".Translate(this.recipe.ProducedThingDef.LabelCap));
				sb.AppendLine("GestatingMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")));
				break;
			case FormingState.Forming:
				sb.AppendLine("GestatingMech".Translate(this.recipe.ProducedThingDef.LabelCap));
				break;
			case FormingState.Formed:
				if (base.BoundPawn != null)
				{
					sb.AppendLine("GestatedMech".Translate(this.recipe.ProducedThingDef.LabelCap, base.BoundPawn.Named("PAWN")) + " (" + "GestatedMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")) + ")");
				}
				break;
			}
			base.AppendInspectionData(sb);
		}
	}
}
