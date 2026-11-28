using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001747 RID: 5959
	public abstract class CompRitualEffect_SpawnOnPawn : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x0600944D RID: 37965 RVA: 0x00360CAC File Offset: 0x0035EEAC
		protected new CompProperties_RitualEffectSpawnOnPawn Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnPawn)this.props;
			}
		}

		// Token: 0x0600944E RID: 37966 RVA: 0x00360CB9 File Offset: 0x0035EEB9
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x0600944F RID: 37967
		protected abstract Pawn GetPawn(LordJob_Ritual ritual);

		// Token: 0x06009450 RID: 37968 RVA: 0x00360CC0 File Offset: 0x0035EEC0
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			if (this.Props.fleckDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 vector = this.props.offset.RotatedBy(pawn.Rotation);
					if (pawn.Rotation == Rot4.East)
					{
						vector += this.Props.eastRotationOffset;
					}
					else if (pawn.Rotation == Rot4.West)
					{
						vector += this.Props.westRotationOffset;
					}
					else if (pawn.Rotation == Rot4.North)
					{
						vector += this.Props.northRotationOffset;
					}
					else if (pawn.Rotation == Rot4.South)
					{
						vector += this.Props.southRotationOffset;
					}
					base.SpawnFleck(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + vector), new float?(pawn.Rotation.AsAngle));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
		}

		// Token: 0x06009451 RID: 37969 RVA: 0x00360E18 File Offset: 0x0035F018
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Mote result = null;
			if (this.Props.moteDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 b = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnMote(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + b));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}

		// Token: 0x06009452 RID: 37970 RVA: 0x00360EC4 File Offset: 0x0035F0C4
		public override Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			Effecter result = null;
			if (this.Props.effecterDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 value = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnEffecter(this.parent.ritual, pawn, new Vector3?(value));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}
	}
}
