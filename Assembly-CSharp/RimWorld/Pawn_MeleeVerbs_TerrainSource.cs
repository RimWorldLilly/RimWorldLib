using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159D RID: 5533
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06008857 RID: 34903 RVA: 0x0031599C File Offset: 0x00313B9C
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x06008858 RID: 34904 RVA: 0x003159A4 File Offset: 0x00313BA4
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x06008859 RID: 34905 RVA: 0x003159A7 File Offset: 0x00313BA7
		public List<Tool> Tools
		{
			get
			{
				TerrainDef terrainDef = this.def;
				if (terrainDef == null)
				{
					return null;
				}
				return terrainDef.tools;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600885A RID: 34906 RVA: 0x003159BA File Offset: 0x00313BBA
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x0600885B RID: 34907 RVA: 0x003159C7 File Offset: 0x00313BC7
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		// Token: 0x0600885C RID: 34908 RVA: 0x003159CE File Offset: 0x00313BCE
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x0600885D RID: 34909 RVA: 0x003159EF File Offset: 0x00313BEF
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600885E RID: 34910 RVA: 0x00315A1B File Offset: 0x00313C1B
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x0600885F RID: 34911 RVA: 0x00315A38 File Offset: 0x00313C38
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		// Token: 0x04005415 RID: 21525
		public Pawn_MeleeVerbs parent;

		// Token: 0x04005416 RID: 21526
		public TerrainDef def;

		// Token: 0x04005417 RID: 21527
		public VerbTracker tracker;
	}
}
