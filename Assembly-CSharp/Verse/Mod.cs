using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000333 RID: 819
	public abstract class Mod
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x000971E8 File Offset: 0x000953E8
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x000971F0 File Offset: 0x000953F0
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00097200 File Offset: 0x00095400
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)));
				return default(T);
			}
			if (this.modSettings != null)
			{
				return (T)((object)this.modSettings);
			}
			this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.FolderName, base.GetType().Name);
			this.modSettings.Mod = this;
			return this.modSettings as T;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x000972C0 File Offset: 0x000954C0
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000972D5 File Offset: 0x000954D5
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x000972D7 File Offset: 0x000954D7
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x040014B9 RID: 5305
		private ModSettings modSettings;

		// Token: 0x040014BA RID: 5306
		private ModContentPack intContent;
	}
}
