using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B30 RID: 6960
	public static class MoteMaker
	{
		// Token: 0x0600B280 RID: 45696 RVA: 0x003ED882 File Offset: 0x003EBA82
		public static Mote MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			return MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale, false, 0f);
		}

		// Token: 0x0600B281 RID: 45697 RVA: 0x003ED89C File Offset: 0x003EBA9C
		public static Mote MakeStaticMote(Vector3 loc, Map map, ThingDef moteDef, float scale = 1f, bool makeOffscreen = false, float exactRot = 0f)
		{
			if (!makeOffscreen)
			{
				if (!loc.ShouldSpawnMotesAt(map, true) || map.moteCounter.Saturated)
				{
					return null;
				}
			}
			else if (!loc.InBounds(map) || map.moteCounter.Saturated)
			{
				return null;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			GenSpawn.Spawn(mote, loc.ToIntVec3(), map, WipeMode.Vanish);
			mote.exactPosition = loc;
			mote.Scale = scale;
			mote.exactRotation = exactRot;
			return mote;
		}

		// Token: 0x0600B282 RID: 45698 RVA: 0x003ED90F File Offset: 0x003EBB0F
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x0600B283 RID: 45699 RVA: 0x003ED920 File Offset: 0x003EBB20
		public static void ThrowText(Vector3 loc, Map map, string text, Color color, float timeBeforeStartFadeout = -1f)
		{
			IntVec3 intVec = loc.ToIntVec3();
			if (!intVec.InBounds(map))
			{
				return;
			}
			MoteText moteText = (MoteText)ThingMaker.MakeThing(ThingDefOf.Mote_Text, null);
			moteText.exactPosition = loc;
			moteText.SetVelocity((float)Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
			moteText.text = text;
			moteText.textColor = color;
			if (timeBeforeStartFadeout >= 0f)
			{
				moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
			}
			GenSpawn.Spawn(moteText, intVec, map, WipeMode.Vanish);
		}

		// Token: 0x0600B284 RID: 45700 RVA: 0x003ED99D File Offset: 0x003EBB9D
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600B285 RID: 45701 RVA: 0x003ED9D0 File Offset: 0x003EBBD0
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x0600B286 RID: 45702 RVA: 0x003EDA20 File Offset: 0x003EBC20
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B, Vector3 offsetA, Vector3 offsetB)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B, offsetA, offsetB);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x0600B287 RID: 45703 RVA: 0x003EDA70 File Offset: 0x003EBC70
		public static Mote MakeAttachedOverlay(Thing thing, ThingDef moteDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.Attach(thing, offset, false);
			mote.Scale = scale;
			mote.exactPosition = thing.DrawPos + offset;
			mote.solidTimeOverride = solidTimeOverride;
			GenSpawn.Spawn(mote, thing.Position, thing.MapHeld, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600B288 RID: 45704 RVA: 0x003EDACC File Offset: 0x003EBCCC
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.exactPosition = pawn.DrawPos;
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x0600B289 RID: 45705 RVA: 0x003EDB38 File Offset: 0x003EBD38
		private static MoteBubble ExistingMoteBubbleOn(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			for (int i = 0; i < 4; i++)
			{
				if ((pawn.Position + MoteMaker.UpRightPattern[i]).InBounds(pawn.Map))
				{
					List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						MoteBubble moteBubble = thingList[j] as MoteBubble;
						if (moteBubble != null && moteBubble.link1.Linked && moteBubble.link1.Target.HasThing && moteBubble.link1.Target == pawn)
						{
							return moteBubble;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600B28A RID: 45706 RVA: 0x003EDBF8 File Offset: 0x003EBDF8
		public static MoteBubble MakeMoodThoughtBubble(Pawn pawn, Thought thought)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (!pawn.Spawned)
			{
				return null;
			}
			float num = thought.MoodOffset();
			if (num == 0f)
			{
				return null;
			}
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					return null;
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing((num > 0f) ? ThingDefOf.Mote_ThoughtGood : ThingDefOf.Mote_ThoughtBad, null);
			moteBubble2.SetupMoteBubble(thought.Icon, null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600B28B RID: 45707 RVA: 0x003EDCBC File Offset: 0x003EBEBC
		public static MoteBubble MakeThoughtBubble(Pawn pawn, string iconPath, bool maintain = false)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				moteBubble.Destroy(DestroyMode.Vanish);
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(maintain ? ThingDefOf.Mote_ForceJobMaintained : ThingDefOf.Mote_ForceJob, null);
			moteBubble2.SetupMoteBubble(ContentFinder<Texture2D>.Get(iconPath, true), null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600B28C RID: 45708 RVA: 0x003EDD2C File Offset: 0x003EBF2C
		public static MoteBubble MakeInteractionBubble(Pawn initiator, Pawn recipient, ThingDef interactionMote, Texture2D symbol, Color? iconColor = null)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(interactionMote, null);
			moteBubble2.SetupMoteBubble(symbol, recipient, iconColor);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600B28D RID: 45709 RVA: 0x003EDDB0 File Offset: 0x003EBFB0
		public static MoteBubble MakeSpeechBubble(Pawn initiator, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(ThingDefOf.Mote_Speech, null);
			moteBubble2.SetupMoteBubble(symbol, null, null);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x0600B28E RID: 45710 RVA: 0x003EDE40 File Offset: 0x003EC040
		public static void ThrowExplosionCell(IntVec3 cell, Map map, ThingDef moteDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
			mote.exactPosition = cell.ToVector3Shifted();
			mote.instanceColor = color;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x0600B28F RID: 45711 RVA: 0x003EDEAC File Offset: 0x003EC0AC
		public static void ThrowExplosionInteriorMote(Vector3 loc, Map map, ThingDef moteDef)
		{
			if (!loc.ShouldSpawnMotesAt(map, true) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
			moteThrown.Scale = Rand.Range(3f, 4.5f);
			moteThrown.rotationRate = Rand.Range(-30f, 30f);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.48f, 0.72f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x0600B290 RID: 45712 RVA: 0x003EDF3D File Offset: 0x003EC13D
		public static void MakeBombardmentMote(IntVec3 cell, Map map, float scale)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 150f * scale;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B291 RID: 45713 RVA: 0x003EDF7D File Offset: 0x003EC17D
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B292 RID: 45714 RVA: 0x003EDFBB File Offset: 0x003EC1BB
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
			mote.exactPosition = cell.ToVector3Shifted();
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B293 RID: 45715 RVA: 0x003EDFF0 File Offset: 0x003EC1F0
		public static Mote MakeConnectingLine(Vector3 start, Vector3 end, ThingDef moteType, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			Mote mote = MoteMaker.MakeStaticMote(start + vector * 0.5f, map, moteType, 1f, false, 0f);
			if (mote != null)
			{
				mote.linearScale = new Vector3(x, 1f, width);
				mote.exactRotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			}
			return mote;
		}

		// Token: 0x04006706 RID: 26374
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};
	}
}
