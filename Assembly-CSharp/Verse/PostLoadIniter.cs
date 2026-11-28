using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000523 RID: 1315
	public class PostLoadIniter
	{
		// Token: 0x060026E4 RID: 9956 RVA: 0x000F177C File Offset: 0x000EF97C
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Registered " + ((s != null) ? s.ToString() : null) + " for post load init, but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.");
				return;
			}
			try
			{
				if (!this.saveablesToPostLoad.Add(s))
				{
					Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + ((s != null) ? s.ToString() : null));
				}
			}
			catch (Exception ex)
			{
				string str = "Could not register an object for post load init: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000F1830 File Offset: 0x000EFA30
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					string str = "Could not do PostLoadInit on ";
					string str2 = exposable.ToStringSafe<IExposable>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000F18F8 File Offset: 0x000EFAF8
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x04001C06 RID: 7174
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
