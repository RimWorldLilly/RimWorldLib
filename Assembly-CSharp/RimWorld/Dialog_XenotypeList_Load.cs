using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDD RID: 7389
	public class Dialog_XenotypeList_Load : Dialog_XenotypeList
	{
		// Token: 0x0600BDE3 RID: 48611 RVA: 0x0044386C File Offset: 0x00441A6C
		public Dialog_XenotypeList_Load(Action<CustomXenotype> xenotypeReturner)
		{
			this.xenotypeReturner = xenotypeReturner;
			this.interactButLabel = "LoadGameButton".Translate();
			this.deleteTipKey = "DeleteThisXenotype";
		}

		// Token: 0x0600BDE4 RID: 48612 RVA: 0x0044389C File Offset: 0x00441A9C
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsFilePathForXenotype(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, delegate
			{
				CustomXenotype obj;
				if (GameDataSaveLoader.TryLoadXenotype(filePath, out obj))
				{
					this.xenotypeReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x04006CBA RID: 27834
		private Action<CustomXenotype> xenotypeReturner;
	}
}
