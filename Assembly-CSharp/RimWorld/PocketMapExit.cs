using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B2C RID: 6956
	public class PocketMapExit : MapPortal
	{
		// Token: 0x1700202A RID: 8234
		// (get) Token: 0x0600B258 RID: 45656 RVA: 0x003ECB24 File Offset: 0x003EAD24
		public override string EnterString
		{
			get
			{
				string key = "ExitPortal";
				MapPortal mapPortal = this.entrance;
				return key.Translate((mapPortal != null) ? mapPortal.def.portal.pocketMapGenerator.label : null);
			}
		}

		// Token: 0x1700202B RID: 8235
		// (get) Token: 0x0600B259 RID: 45657 RVA: 0x003ECB5B File Offset: 0x003EAD5B
		public override string CancelEnterString
		{
			get
			{
				return "CommandCancelExitPortal".Translate();
			}
		}

		// Token: 0x1700202C RID: 8236
		// (get) Token: 0x0600B25A RID: 45658 RVA: 0x003ECB6C File Offset: 0x003EAD6C
		protected override Texture2D EnterTex
		{
			get
			{
				return PocketMapExit.ExitMapTex.Texture;
			}
		}

		// Token: 0x0600B25B RID: 45659 RVA: 0x003ECB78 File Offset: 0x003EAD78
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				if (PocketMapUtility.currentlyGeneratingPortal == null)
				{
					Log.Error("Pocket map exit could not find map portal to connect to");
					return;
				}
				this.entrance = PocketMapUtility.currentlyGeneratingPortal;
				this.entrance.exit = this;
			}
		}

		// Token: 0x0600B25C RID: 45660 RVA: 0x003ECBAE File Offset: 0x003EADAE
		public override Map GetOtherMap()
		{
			return this.entrance.Map;
		}

		// Token: 0x0600B25D RID: 45661 RVA: 0x003ECBBB File Offset: 0x003EADBB
		public override IntVec3 GetDestinationLocation()
		{
			return this.entrance.Position;
		}

		// Token: 0x0600B25E RID: 45662 RVA: 0x003ECBC8 File Offset: 0x003EADC8
		public override void OnEntered(Pawn pawn)
		{
			base.Notify_ThingAdded(pawn);
			if (Find.CurrentMap != base.Map)
			{
				if (Find.CurrentMap == this.entrance.Map)
				{
					SoundDef traverseSound = this.entrance.def.portal.traverseSound;
					if (traverseSound == null)
					{
						return;
					}
					traverseSound.PlayOneShot(this.entrance);
				}
				return;
			}
			SoundDef traverseSound2 = this.entrance.def.portal.traverseSound;
			if (traverseSound2 == null)
			{
				return;
			}
			traverseSound2.PlayOneShot(this);
		}

		// Token: 0x0600B25F RID: 45663 RVA: 0x003ECC4B File Offset: 0x003EAE4B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			PocketMapExit.<GetGizmos>d__13 <GetGizmos>d__ = new PocketMapExit.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B260 RID: 45664 RVA: 0x003ECC5B File Offset: 0x003EAE5B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapPortal>(ref this.entrance, "pitGate", false);
		}

		// Token: 0x040066ED RID: 26349
		private static readonly CachedTexture ExitMapTex = new CachedTexture("UI/Commands/ExitCave");

		// Token: 0x040066EE RID: 26350
		private static readonly CachedTexture ViewEntranceTex = new CachedTexture("UI/Commands/ViewCave");

		// Token: 0x040066EF RID: 26351
		public MapPortal entrance;
	}
}
