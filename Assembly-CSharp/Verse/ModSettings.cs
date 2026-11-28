using System;

namespace Verse
{
	// Token: 0x02000348 RID: 840
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x0009CCFC File Offset: 0x0009AEFC
		// (set) Token: 0x06001A29 RID: 6697 RVA: 0x0009CD04 File Offset: 0x0009AF04
		public Mod Mod { get; internal set; }

		// Token: 0x06001A2A RID: 6698 RVA: 0x0009CD0D File Offset: 0x0009AF0D
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0009CD0F File Offset: 0x0009AF0F
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
