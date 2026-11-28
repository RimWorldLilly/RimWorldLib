using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191E RID: 6430
	public class CompObelisk_Abductor : CompObelisk_ExplodingSpawner
	{
		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x0600A143 RID: 41283 RVA: 0x00399F28 File Offset: 0x00398128
		public LabyrinthMapComponent Labyrinth
		{
			get
			{
				LabyrinthMapComponent result;
				if ((result = this.mapComp) == null)
				{
					result = (this.mapComp = this.labyrinthMap.GetComponent<LabyrinthMapComponent>());
				}
				return result;
			}
		}

		// Token: 0x0600A144 RID: 41284 RVA: 0x00399F54 File Offset: 0x00398154
		public override void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false)
		{
			if (!triggeredByPlayer)
			{
				if (GenTicks.TicksGame < this.cooldownTick)
				{
					return;
				}
				this.cooldownTick = GenTicks.TicksGame + Mathf.RoundToInt(CompObelisk_Abductor.CooldownDaysRange.RandomInRange * 60000f);
			}
			if (this.labyrinthMap != null)
			{
				this.Labyrinth.TeleportToLabyrinth(interactor);
				CompObelisk_Abductor.SendTriggeredLetter(interactor);
				return;
			}
			this.instigator = interactor;
			interactor.stances.stunner.StunFor(240, null, false, false, false);
			this.interactSkipTick = GenTicks.TicksGame + 240;
			this.effects[interactor] = EffecterDefOf.AbductWarmup_Pawn.SpawnAttached(interactor, interactor.MapHeld, 1f);
			if (this.buildingEffecter == null)
			{
				this.buildingEffecter = EffecterDefOf.AbductWarmup_Monolith.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			CompObelisk_Abductor.SendDisappearingLetter(interactor);
		}

		// Token: 0x0600A145 RID: 41285 RVA: 0x0039A048 File Offset: 0x00398248
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			Find.LetterStack.ReceiveLetter("ObeliskAbductorLetterLabel".Translate(), "ObeliskAbductorLetter".Translate(), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
			this.pointsRemaining *= 0.15f;
			int num = 0;
			int num2 = 0;
			foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (CompObelisk_Abductor.IsValidColonist(pawn))
				{
					num++;
				}
				else if (CompObelisk_Abductor.IsValidOther(pawn))
				{
					num2++;
				}
			}
			this.colonists = Mathf.Max(Mathf.FloorToInt((float)num * 0.5f), 1);
			this.others = Mathf.Max(Mathf.FloorToInt((float)num2 * 0.2f), 1);
			foreach (Pawn pawn2 in this.parent.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (CompObelisk_Abductor.IsValidColonist(pawn2) && this.colonists > 0)
				{
					this.colonists--;
					this.teleporting.Add(pawn2);
					pawn2.stances.stunner.StunFor(2400, null, false, false, false);
					this.effects[pawn2] = EffecterDefOf.AbductWarmup_Pawn.SpawnAttached(pawn2, pawn2.MapHeld, 1f);
					if (this.buildingEffecter == null)
					{
						this.buildingEffecter = EffecterDefOf.AbductWarmup_Monolith.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
					}
				}
				else if (CompObelisk_Abductor.IsValidOther(pawn2) && this.others > 0)
				{
					this.others--;
					this.teleporting.Add(pawn2);
				}
			}
			this.instigator = null;
			this.nextTeleportTick = GenTicks.TicksGame + 240;
			this.lastTeleportTick = this.nextTeleportTick;
		}

		// Token: 0x0600A146 RID: 41286 RVA: 0x0039A278 File Offset: 0x00398478
		public override void CompTick()
		{
			base.CompTick();
			if (this.interactSkipTick != 0 && GenTicks.TicksGame >= this.interactSkipTick)
			{
				this.interactSkipTick = 0;
				if (this.labyrinthMap == null)
				{
					this.GenerateLabyrinth(this.instigator);
				}
				CompObelisk_Abductor.SendTriggeredLetter(this.instigator);
				this.instigator = null;
			}
			this.VFXTick();
			if (this.activated)
			{
				this.ActivatedTick();
			}
		}

		// Token: 0x0600A147 RID: 41287 RVA: 0x0039A2E4 File Offset: 0x003984E4
		private void VFXTick()
		{
			foreach (KeyValuePair<Pawn, Effecter> keyValuePair in this.effects)
			{
				Pawn pawn;
				Effecter effecter;
				keyValuePair.Deconstruct(out pawn, out effecter);
				Pawn pawn2 = pawn;
				Effecter effecter2 = effecter;
				if (pawn2.Map == this.parent.Map)
				{
					effecter2.EffectTick(pawn2, pawn2);
				}
				else
				{
					effecter2.Cleanup();
					CompObelisk_Abductor.tmpRemoveEffectors.Add(pawn2);
				}
			}
			this.effects.RemoveRange(CompObelisk_Abductor.tmpRemoveEffectors);
			CompObelisk_Abductor.tmpRemoveEffectors.Clear();
			if (this.effects.Count == 0 && this.buildingEffecter != null)
			{
				this.buildingEffecter.Cleanup();
				this.buildingEffecter = null;
			}
		}

		// Token: 0x0600A148 RID: 41288 RVA: 0x0039A3BC File Offset: 0x003985BC
		private void ActivatedTick()
		{
			if (this.explodeTick > 0)
			{
				return;
			}
			if (GenTicks.TicksGame < this.nextTeleportTick)
			{
				return;
			}
			if (this.labyrinthMap == null && !this.generating)
			{
				this.lastTeleportTick = GenTicks.TicksGame;
				this.GenerateLabyrinth(null);
				return;
			}
			if (this.labyrinthMap == null)
			{
				this.nextTeleportTick = GenTicks.TicksGame + CompObelisk_Abductor.TeleportDelayTicksRange.RandomInRange;
				this.lastTeleportTick = GenTicks.TicksGame;
				return;
			}
			this.nextTeleportTick = GenTicks.TicksGame + CompObelisk_Abductor.TeleportDelayTicksRange.RandomInRange;
			if (this.explodeTick < 0 && GenTicks.TicksGame >= this.lastTeleportTick + 180)
			{
				base.PrepareExplosion();
			}
			for (int i = this.teleporting.Count - 1; i >= 0; i--)
			{
				if (this.teleporting[i].DestroyedOrNull())
				{
					this.teleporting.RemoveAt(i);
				}
			}
			if (this.teleporting.Empty<Pawn>())
			{
				return;
			}
			Pawn pawn = this.teleporting.RandomElement<Pawn>();
			this.teleporting.Remove(pawn);
			if (CompObelisk_Abductor.IsValidColonist(pawn))
			{
				this.colonists--;
			}
			else if (CompObelisk_Abductor.IsValidHostile(pawn))
			{
				this.pointsRemaining -= pawn.kindDef.combatPower;
			}
			else
			{
				this.others--;
			}
			this.Labyrinth.TeleportToLabyrinth(pawn);
			this.lastTeleportTick = GenTicks.TicksGame;
			pawn.stances.stunner.StopStun();
		}

		// Token: 0x0600A149 RID: 41289 RVA: 0x0039A53C File Offset: 0x0039873C
		private void GenerateLabyrinth(Pawn interactor)
		{
			this.generating = true;
			LongEventHandler.QueueLongEvent(delegate()
			{
				this.labyrinthMap = PocketMapUtility.GeneratePocketMap(new IntVec3(90, 1, 90), MapGeneratorDefOf.Labyrinth, null, this.parent.MapHeld);
				LabyrinthMapComponent component = this.labyrinthMap.GetComponent<LabyrinthMapComponent>();
				component.abductorObelisk = (Building)this.parent;
				component.sourceMap = this.parent.Map;
			}, "GeneratingLabyrinth", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), false, false, delegate()
			{
				this.generating = false;
				if (interactor != null)
				{
					this.Labyrinth.TeleportToLabyrinth(interactor);
				}
				CameraJumper.TryJump(interactor, CameraJumper.MovementMode.Cut);
			});
		}

		// Token: 0x0600A14A RID: 41290 RVA: 0x0039A598 File Offset: 0x00398798
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.labyrinthMap == null)
			{
				return;
			}
			if (this.labyrinthMap.mapPawns.AnyColonistSpawned || this.labyrinthMap.mapPawns.SlavesOfColonySpawned.Any<Pawn>())
			{
				return;
			}
			PocketMapUtility.DestroyPocketMap(this.labyrinthMap);
			this.Notify_MapDestroyed();
		}

		// Token: 0x0600A14B RID: 41291 RVA: 0x0039A5F1 File Offset: 0x003987F1
		public void Notify_MapDestroyed()
		{
			this.labyrinthMap = null;
			this.mapComp = null;
		}

		// Token: 0x0600A14C RID: 41292 RVA: 0x0039A604 File Offset: 0x00398804
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.cooldownTick, "cooldownTick", 0, false);
			Scribe_Values.Look<int>(ref this.nextTeleportTick, "nextTeleport", 0, false);
			Scribe_Values.Look<int>(ref this.lastTeleportTick, "lastTeleport", 0, false);
			Scribe_Values.Look<int>(ref this.interactSkipTick, "interactSendTick", 0, false);
			Scribe_Values.Look<int>(ref this.colonists, "colonists", 0, false);
			Scribe_Values.Look<int>(ref this.others, "others", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.teleporting, "teleporting", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Map>(ref this.labyrinthMap, "labyrinthMap", false);
			Scribe_References.Look<Pawn>(ref this.instigator, "storedInstigator", false);
		}

		// Token: 0x0600A14D RID: 41293 RVA: 0x0039A6BB File Offset: 0x003988BB
		private static bool IsValidColonist(Pawn pawn)
		{
			return pawn.IsColonist || pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony;
		}

		// Token: 0x0600A14E RID: 41294 RVA: 0x0039A6D5 File Offset: 0x003988D5
		private static bool IsValidHostile(Pawn pawn)
		{
			return pawn.HostileTo(Faction.OfPlayer) && !pawn.IsOnHoldingPlatform;
		}

		// Token: 0x0600A14F RID: 41295 RVA: 0x0039A6EF File Offset: 0x003988EF
		private static bool IsValidOther(Pawn pawn)
		{
			return pawn.IsAnimal || pawn.IsColonyMech;
		}

		// Token: 0x0600A150 RID: 41296 RVA: 0x0039A704 File Offset: 0x00398904
		private static void SendDisappearingLetter(Pawn interactor)
		{
			TaggedString label = "ObeliskAbductedDisappearingLetterLabel".Translate(interactor.Named("PAWN"));
			TaggedString text = "ObeliskAbductedDisappearingLetter".Translate(interactor.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, interactor, null, null, null, null, 0, true);
		}

		// Token: 0x0600A151 RID: 41297 RVA: 0x0039A75C File Offset: 0x0039895C
		private static void SendTriggeredLetter(Pawn interactor)
		{
			TaggedString label = "ObeliskAbductedLetterLabel".Translate(interactor.Named("PAWN"));
			TaggedString text = "ObeliskAbductedLetter".Translate(interactor.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, interactor, null, null, null, null, 0, true);
		}

		// Token: 0x04005F35 RID: 24373
		private int cooldownTick;

		// Token: 0x04005F36 RID: 24374
		private Map labyrinthMap;

		// Token: 0x04005F37 RID: 24375
		private int nextTeleportTick;

		// Token: 0x04005F38 RID: 24376
		private int lastTeleportTick;

		// Token: 0x04005F39 RID: 24377
		private int interactSkipTick;

		// Token: 0x04005F3A RID: 24378
		private int colonists;

		// Token: 0x04005F3B RID: 24379
		private int others;

		// Token: 0x04005F3C RID: 24380
		private Pawn instigator;

		// Token: 0x04005F3D RID: 24381
		private Dictionary<Pawn, Effecter> effects = new Dictionary<Pawn, Effecter>();

		// Token: 0x04005F3E RID: 24382
		private Effecter buildingEffecter;

		// Token: 0x04005F3F RID: 24383
		private List<Pawn> teleporting = new List<Pawn>();

		// Token: 0x04005F40 RID: 24384
		private LabyrinthMapComponent mapComp;

		// Token: 0x04005F41 RID: 24385
		private bool generating;

		// Token: 0x04005F42 RID: 24386
		private const int LabyrinthSize = 90;

		// Token: 0x04005F43 RID: 24387
		private const int LastTeleportExplosionDelay = 180;

		// Token: 0x04005F44 RID: 24388
		private const int InteractSkipDelayTicks = 240;

		// Token: 0x04005F45 RID: 24389
		private const int ActivityTeleportDelayTicks = 240;

		// Token: 0x04005F46 RID: 24390
		private const float ColonistFactor = 0.5f;

		// Token: 0x04005F47 RID: 24391
		private const float OtherFactor = 0.2f;

		// Token: 0x04005F48 RID: 24392
		private const float HostilePointsFactor = 0.15f;

		// Token: 0x04005F49 RID: 24393
		private static readonly FloatRange CooldownDaysRange = new FloatRange(15f, 80f);

		// Token: 0x04005F4A RID: 24394
		private static readonly IntRange TeleportDelayTicksRange = new IntRange(10, 30);

		// Token: 0x04005F4B RID: 24395
		private static readonly List<Pawn> tmpRemoveEffectors = new List<Pawn>();
	}
}
