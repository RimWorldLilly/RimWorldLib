using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using Verse;

// Token: 0x0200000E RID: 14
public class LayoutWorkerComplex_Mechanitor : LayoutWorkerComplex
{
	// Token: 0x06000040 RID: 64 RVA: 0x0000344C File Offset: 0x0000164C
	public LayoutWorkerComplex_Mechanitor(LayoutDef def) : base(def)
	{
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003460 File Offset: 0x00001660
	public override Faction GetFixedHostileFactionForThreats()
	{
		return Faction.OfMechanoids;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003468 File Offset: 0x00001668
	protected override void PreSpawnThreats(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		LayoutWorkerComplex_Mechanitor.<>c__DisplayClass6_0 CS$<>8__locals1 = new LayoutWorkerComplex_Mechanitor.<>c__DisplayClass6_0();
		base.PreSpawnThreats(rooms, map, allSpawnedThings);
		this.tmpAllRoomRects.Clear();
		this.tmpAllRoomRects.AddRange((from r in rooms
		where r.requiredDef == null
		select r).SelectMany((LayoutRoom r) => r.rects));
		CS$<>8__locals1.bounds = this.tmpAllRoomRects[0];
		for (int i = 0; i < this.tmpAllRoomRects.Count; i++)
		{
			CS$<>8__locals1.bounds = CS$<>8__locals1.bounds.Encapsulate(this.tmpAllRoomRects[i]);
		}
		bool flag = false;
		foreach (CellRect room in this.tmpAllRoomRects.OrderBy(new Func<CellRect, float>(CS$<>8__locals1.<PreSpawnThreats>g__OrderRoomsBy|2)))
		{
			Building_AncientCryptosleepPod item;
			if (this.TryPlaceDeceasedMechanitor(room, map, out item))
			{
				allSpawnedThings.Add(item);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Log.Error("Failed to place mechanitor in ancient mechanitor complex.");
		}
		this.tmpAllRoomRects.Clear();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000035A8 File Offset: 0x000017A8
	private bool TryPlaceDeceasedMechanitor(CellRect room, Map map, out Building_AncientCryptosleepPod casket)
	{
		LayoutWorkerComplex_Mechanitor.<>c__DisplayClass7_0 CS$<>8__locals1;
		CS$<>8__locals1.map = map;
		foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
		{
			if (LayoutWorkerComplex_Mechanitor.<TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|7_0(intVec, ref CS$<>8__locals1))
			{
				casket = (Building_AncientCryptosleepPod)GenSpawn.Spawn(ThingDefOf.AncientCryptosleepPod, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				casket.openedSignal = "MechanitorCasketOpened" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Mechanitor_Basic, Faction.OfAncients, PawnGenerationContext.NonPlayer, new PlanetTile?(CS$<>8__locals1.map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.Corpse.Age = LayoutWorkerComplex_Mechanitor.RandomMechanitorCorpseAge.RandomInRange;
				pawn.relations.hidePawnRelations = true;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				casket.TryAcceptThing(pawn.Corpse, false);
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = casket.openedSignal;
				signalAction_Message.message = "MessageMechanitorCasketOpened".Translate(pawn, HediffDefOf.MechlinkImplant);
				signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
				signalAction_Message.lookTargets = pawn.Corpse;
				GenSpawn.Spawn(signalAction_Message, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter.signalTag = casket.openedSignal;
				signalAction_Letter.letterDef = LetterDefOf.NeutralEvent;
				signalAction_Letter.letterLabelKey = "LetterLabelMechanitorCasketOpened";
				signalAction_Letter.letterMessageKey = "LetterMechanitorCasketOpened";
				signalAction_Letter.fixedPawnReference = pawn;
				signalAction_Letter.lookTargets = pawn.Corpse;
				GenSpawn.Spawn(signalAction_Letter, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
				signalAction_SoundOneShot.signalTag = casket.openedSignal;
				signalAction_SoundOneShot.sound = SoundDefOf.MechlinkCorpseReveal;
				GenSpawn.Spawn(signalAction_SoundOneShot, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
				triggerUnfogged.signalTag = "MechanitorCasketUnfogged" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				GenSpawn.Spawn(triggerUnfogged, casket.Position, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter2 = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter2.signalTag = triggerUnfogged.signalTag;
				signalAction_Letter2.letterDef = LetterDefOf.NeutralEvent;
				signalAction_Letter2.letterLabelKey = "LetterLabelMechanitorCasketFound";
				signalAction_Letter2.letterMessageKey = "LetterMechanitorCasketFound";
				GenSpawn.Spawn(signalAction_Letter2, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				ScatterDebrisUtility.ScatterFilthAroundThing(casket, CS$<>8__locals1.map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				return true;
			}
		}
		casket = null;
		return false;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003930 File Offset: 0x00001B30
	[CompilerGenerated]
	internal static bool <TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|7_0(IntVec3 cell, ref LayoutWorkerComplex_Mechanitor.<>c__DisplayClass7_0 A_1)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCryptosleepPod.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(A_1.map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04000011 RID: 17
	private const string MechanitorCasketOpenedSignal = "MechanitorCasketOpened";

	// Token: 0x04000012 RID: 18
	private const string MechanitorCasketUnfoggedSignal = "MechanitorCasketUnfogged";

	// Token: 0x04000013 RID: 19
	private static readonly IntRange RandomMechanitorCorpseAge = new IntRange(0, 360000000);

	// Token: 0x04000014 RID: 20
	private readonly List<CellRect> tmpAllRoomRects = new List<CellRect>();
}
