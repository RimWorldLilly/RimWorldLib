using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBB RID: 4027
	public class PawnRelationWorker
	{
		// Token: 0x06006653 RID: 26195 RVA: 0x002457DC File Offset: 0x002439DC
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				PawnRelationDef pawnRelationDef = this.def;
				throw new NotImplementedException(((pawnRelationDef != null) ? pawnRelationDef.ToString() : null) + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x0024582A File Offset: 0x00243A2A
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x00245834 File Offset: 0x00243A34
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			PawnRelationDef pawnRelationDef = this.def;
			throw new NotImplementedException(((pawnRelationDef != null) ? pawnRelationDef.ToString() : null) + " lacks CreateRelation implementation.");
		}

		// Token: 0x06006656 RID: 26198 RVA: 0x00245884 File Offset: 0x00243A84
		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (other.IsDuplicate)
			{
				return 0f;
			}
			if (generated.Faction != other.Faction)
			{
				num *= 0.65f;
			}
			if (generated.HostileTo(other))
			{
				num *= 0.7f;
			}
			if (other.Faction != null && other.Faction.IsPlayer && (generated.Faction == null || !generated.Faction.IsPlayer))
			{
				num *= 0.5f;
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			if (other == request.ExtraPawnForExtraRelationChance)
			{
				num *= request.RelationWithExtraPawnChanceFactor;
			}
			TechLevel techLevel = (generated.Faction != null) ? generated.Faction.def.techLevel : TechLevel.Undefined;
			TechLevel techLevel2 = (other.Faction != null) ? other.Faction.def.techLevel : TechLevel.Undefined;
			if (techLevel != TechLevel.Undefined && techLevel2 != TechLevel.Undefined && techLevel != techLevel2)
			{
				num *= 0.85f;
			}
			if ((techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse()) || (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse()))
			{
				num *= 0.03f;
			}
			return num;
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x002459A4 File Offset: 0x00243BA4
		public virtual void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x002459A6 File Offset: 0x00243BA6
		public virtual void OnRelationRemoved(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x002459A8 File Offset: 0x00243BA8
		public virtual void Notify_PostRemovedByDeath(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x0600665A RID: 26202 RVA: 0x002459AA File Offset: 0x00243BAA
		public virtual void Notify_PostRemovedLeftBehind(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x04003EB5 RID: 16053
		public PawnRelationDef def;
	}
}
