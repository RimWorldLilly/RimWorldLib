using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000662 RID: 1634
	public static class DialogDatabase
	{
		// Token: 0x060033DD RID: 13277 RVA: 0x0014DA81 File Offset: 0x0014BC81
		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x0014DA9C File Offset: 0x0014BC9C
		private static void LoadAllDialog()
		{
			DialogDatabase.Nodes.Clear();
			foreach (Object @object in Resources.LoadAll("Dialog", typeof(TextAsset)))
			{
				TextAsset ass = @object as TextAsset;
				if (@object.name == "BaseEncounters" || @object.name == "GeneratedDialogs")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.BaseEncounters);
				}
				if (@object.name == "InsanityBattles")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.InsanityBattles);
				}
				if (@object.name == "SpecialEncounters")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.Special);
				}
			}
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				diaNodeMold.PostLoad();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0014DBB0 File Offset: 0x0014BDB0
		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.isRoot && (!diaNodeMold.unique || !diaNodeMold.used) && diaNodeMold.nodeType == NType)
				{
					list.Add(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0014DC30 File Offset: 0x0014BE30
		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.name == NodeName)
				{
					return diaNodeMold;
				}
			}
			foreach (DiaNodeList diaNodeList in DialogDatabase.NodeLists)
			{
				if (diaNodeList.Name == NodeName)
				{
					return diaNodeList.RandomNodeFromList();
				}
			}
			Log.Error("Did not find node named '" + NodeName + "'.");
			return null;
		}

		// Token: 0x040021B2 RID: 8626
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x040021B3 RID: 8627
		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();
	}
}
