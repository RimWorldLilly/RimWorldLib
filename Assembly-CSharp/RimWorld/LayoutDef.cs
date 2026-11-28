using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F87 RID: 3975
	public class LayoutDef : Def
	{
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x060065A7 RID: 26023 RVA: 0x00243064 File Offset: 0x00241264
		public LayoutWorker Worker
		{
			get
			{
				LayoutWorker result;
				if ((result = this.layoutWorker) == null)
				{
					result = (this.layoutWorker = (LayoutWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x04003C71 RID: 15473
		public List<LayoutDef.LayoutRoomWeight> roomDefs;

		// Token: 0x04003C72 RID: 15474
		public Type workerClass;

		// Token: 0x04003C73 RID: 15475
		public bool clearRoomsEntirely;

		// Token: 0x04003C74 RID: 15476
		public bool canHaveMultipleLayoutsInRoom;

		// Token: 0x04003C75 RID: 15477
		public float multipleLayoutRoomChance = 0.15f;

		// Token: 0x04003C76 RID: 15478
		public bool shouldDamage;

		// Token: 0x04003C77 RID: 15479
		public bool noRoof;

		// Token: 0x04003C78 RID: 15480
		public float adjacentRoomMergeChance = 0.65f;

		// Token: 0x04003C79 RID: 15481
		public float borderDoorRemoveChance = 0.8f;

		// Token: 0x04003C7A RID: 15482
		public int minRoomWidth = 6;

		// Token: 0x04003C7B RID: 15483
		public int minRoomHeight = 6;

		// Token: 0x04003C7C RID: 15484
		public List<LayoutScatterParms> junkScaterrers = new List<LayoutScatterParms>();

		// Token: 0x04003C7D RID: 15485
		public List<LayoutScatterTerrainParms> scatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x04003C7E RID: 15486
		public List<LayoutFillEdgesParms> fillEdges = new List<LayoutFillEdgesParms>();

		// Token: 0x04003C7F RID: 15487
		public List<LayoutFillInteriorParms> fillInterior = new List<LayoutFillInteriorParms>();

		// Token: 0x04003C80 RID: 15488
		public List<LayoutWallAttatchmentParms> wallAttachments = new List<LayoutWallAttatchmentParms>();

		// Token: 0x04003C81 RID: 15489
		public List<LayoutPrefabParms> prefabs = new List<LayoutPrefabParms>();

		// Token: 0x04003C82 RID: 15490
		public List<LayoutPartParms> parts = new List<LayoutPartParms>();

		// Token: 0x04003C83 RID: 15491
		private LayoutWorker layoutWorker;

		// Token: 0x02002F69 RID: 12137
		public class LayoutRoomWeight
		{
			// Token: 0x06011EB3 RID: 73395 RVA: 0x0061D72B File Offset: 0x0061B92B
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
			}

			// Token: 0x0400C0BA RID: 49338
			public LayoutRoomDef def;

			// Token: 0x0400C0BB RID: 49339
			public float weight = 1f;

			// Token: 0x0400C0BC RID: 49340
			public IntRange countRange = new IntRange(0, int.MaxValue);
		}
	}
}
