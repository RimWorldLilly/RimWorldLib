using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE9 RID: 6889
	public class MonolithIncoming : GroundSpawner
	{
		// Token: 0x17001FB9 RID: 8121
		// (get) Token: 0x0600AFFC RID: 45052 RVA: 0x003E05F8 File Offset: 0x003DE7F8
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				return new IntRange(EffecterDefOf.VoidStructureIncoming.maintainTicks);
			}
		}

		// Token: 0x17001FBA RID: 8122
		// (get) Token: 0x0600AFFD RID: 45053 RVA: 0x003E0609 File Offset: 0x003DE809
		protected override SoundDef SustainerSound
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001FBB RID: 8123
		// (get) Token: 0x0600AFFE RID: 45054 RVA: 0x003E060C File Offset: 0x003DE80C
		protected override bool SpawnRubble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AFFF RID: 45055 RVA: 0x003E0610 File Offset: 0x003DE810
		protected override void Spawn(Map map, IntVec3 pos)
		{
			Building_VoidMonolith t = Find.Anomaly.SpawnNewMonolith(pos, map);
			TaggedString taggedString = "MonolithArrivalText".Translate();
			if (Find.Anomaly.Level > 0)
			{
				taggedString += "\n\n" + "MonolithArrivalTextExt".Translate();
			}
			Find.LetterStack.ReceiveLetter("MonolithArrivalLabel".Translate(), taggedString, LetterDefOf.NeutralEvent, t, null, null, null, null, 0, true);
		}
	}
}
