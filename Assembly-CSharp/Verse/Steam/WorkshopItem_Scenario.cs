using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x0200074B RID: 1867
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06003B38 RID: 15160 RVA: 0x0017A012 File Offset: 0x00178212
		// (set) Token: 0x06003B39 RID: 15161 RVA: 0x0017A01A File Offset: 0x0017821A
		public override PublishedFileId_t PublishedFileId
		{
			get
			{
				return base.PublishedFileId;
			}
			set
			{
				base.PublishedFileId = value;
				if (this.cachedScenario != null)
				{
					this.cachedScenario.SetPublishedFileId(value);
				}
			}
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x0017A037 File Offset: 0x00178237
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0017A050 File Offset: 0x00178250
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario(base.Directory.GetFiles("*.rsc").First((FileInfo fi) => fi.Extension == ".rsc").FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x040026D2 RID: 9938
		private Scenario cachedScenario;
	}
}
