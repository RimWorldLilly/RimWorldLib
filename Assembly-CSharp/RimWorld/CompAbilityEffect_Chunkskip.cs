using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013DF RID: 5087
	public class CompAbilityEffect_Chunkskip : CompAbilityEffect
	{
		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06007E7A RID: 32378 RVA: 0x002D304A File Offset: 0x002D124A
		public new CompProperties_AbilityChunkskip Props
		{
			get
			{
				return (CompProperties_AbilityChunkskip)this.props;
			}
		}

		// Token: 0x06007E7B RID: 32379 RVA: 0x002D3058 File Offset: 0x002D1258
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IEnumerable<Thing> enumerable = this.FindClosestChunks(target);
			Map map = this.parent.pawn.Map;
			foreach (Thing thing in enumerable)
			{
				IntVec3 intVec;
				if (this.FindFreeCell(target.Cell, map, out intVec))
				{
					AbilityUtility.DoClamor(thing.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
					AbilityUtility.DoClamor(intVec, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(thing.Position, map, 0.72f), thing.Position, 60, null);
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, map, 0.72f), intVec, 60, null);
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Chunkskip.DustColor);
					thing.Position = intVec;
					CompAbilityEffect_Teleport.SendSkipUsedSignal(intVec, this.parent.pawn);
				}
			}
			SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, map, false));
			base.Apply(target, dest);
		}

		// Token: 0x06007E7C RID: 32380 RVA: 0x002D31DC File Offset: 0x002D13DC
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_Chunkskip.<GetPreCastActions>d__4 <GetPreCastActions>d__ = new CompAbilityEffect_Chunkskip.<GetPreCastActions>d__4(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007E7D RID: 32381 RVA: 0x002D31EC File Offset: 0x002D13EC
		private IEnumerable<Thing> FindClosestChunks(LocalTargetInfo target)
		{
			if (this.lastChunkUpdateFrame == Time.frameCount && this.foundChunksTemp != null)
			{
				return this.foundChunksTemp;
			}
			if (this.foundChunksTemp == null)
			{
				this.foundChunksTemp = new List<Thing>();
			}
			this.foundChunksTemp.Clear();
			RegionTraverser.BreadthFirstTraverse(target.Cell, this.parent.pawn.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Chunk);
				int num = 0;
				while (num < list.Count && this.foundChunksTemp.Count < this.Props.chunkCount)
				{
					Thing thing = list[num];
					if (!thing.Fogged() && !this.foundChunksTemp.Contains(thing))
					{
						this.foundChunksTemp.Add(thing);
					}
					num++;
				}
				return this.foundChunksTemp.Count >= this.Props.chunkCount;
			}, 999999, RegionType.Set_All);
			this.lastChunkUpdateFrame = Time.frameCount;
			return this.foundChunksTemp;
		}

		// Token: 0x06007E7E RID: 32382 RVA: 0x002D3294 File Offset: 0x002D1494
		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result)
		{
			return CellFinder.TryFindRandomCellNear(target, map, Mathf.RoundToInt(this.Props.scatterRadius) - 1, (IntVec3 cell) => CompAbilityEffect_WithDest.CanTeleportThingTo(cell, map) && GenSight.LineOfSight(cell, target, map, true, null, 0, 0), out result, -1);
		}

		// Token: 0x06007E7F RID: 32383 RVA: 0x002D32E8 File Offset: 0x002D14E8
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			foreach (Thing t in this.FindClosestChunks(target))
			{
				GenDraw.DrawLineBetween(t.TrueCenter(), target.CenterVector3);
				GenDraw.DrawTargetHighlight(t);
			}
			GenDraw.DrawRadiusRing(target.Cell, this.Props.scatterRadius);
		}

		// Token: 0x06007E80 RID: 32384 RVA: 0x002D3364 File Offset: 0x002D1564
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (!target.Cell.Standable(this.parent.pawn.Map))
			{
				return false;
			}
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				return false;
			}
			if (!this.FindClosestChunks(target).Any<Thing>())
			{
				return false;
			}
			IntVec3 intVec;
			if (!this.FindFreeCell(target.Cell, this.parent.pawn.Map, out intVec))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityNotEnoughFreeSpace".Translate(), this.parent.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.Valid(target, throwMessages);
		}

		// Token: 0x06007E81 RID: 32385 RVA: 0x002D3448 File Offset: 0x002D1648
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IntVec3 intVec;
			return this.FindClosestChunks(target).Any<Thing>() && this.FindFreeCell(target.Cell, this.parent.pawn.Map, out intVec) && base.CanApplyOn(target, dest);
		}

		// Token: 0x06007E82 RID: 32386 RVA: 0x002D3490 File Offset: 0x002D1690
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.IsValid && !target.Cell.Filled(this.parent.pawn.Map) && !this.FindClosestChunks(target).Any<Thing>())
			{
				return "AbilityNoChunkToSkip".Translate();
			}
			return base.ExtraLabelMouseAttachment(target);
		}

		// Token: 0x04004F60 RID: 20320
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 3f);

		// Token: 0x04004F61 RID: 20321
		private List<Thing> foundChunksTemp;

		// Token: 0x04004F62 RID: 20322
		private int lastChunkUpdateFrame;
	}
}
