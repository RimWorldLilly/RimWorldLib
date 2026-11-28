using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000520 RID: 1312
	public class CrossRefHandler
	{
		// Token: 0x060026CF RID: 9935 RVA: 0x000F0A40 File Offset: 0x000EEC40
		public void RegisterForCrossRefResolve(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Registered " + ((s != null) ? s.ToString() : null) + " for cross ref resolve, but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (s == null)
			{
				return;
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("RegisterForCrossRefResolve " + ((s != null) ? s.GetType().ToString() : "null"));
			}
			this.crossReferencingExposables.Add(s);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000F0AC4 File Offset: 0x000EECC4
		public void ResolveAllCrossReferences()
		{
			Scribe.mode = LoadSaveMode.ResolvingCrossRefs;
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Register the saveables all so we can find them later");
			}
			foreach (IExposable exposable in this.crossReferencingExposables)
			{
				ILoadReferenceable loadReferenceable = exposable as ILoadReferenceable;
				if (loadReferenceable != null)
				{
					if (DebugViewSettings.logMapLoad)
					{
						string str = "RegisterLoaded ";
						Type type = loadReferenceable.GetType();
						LogSimple.Message(str + ((type != null) ? type.ToString() : null));
					}
					this.loadedObjectDirectory.RegisterLoaded(loadReferenceable);
				}
			}
			if (DebugViewSettings.logMapLoad)
			{
				LogSimple.Message("==================Fill all cross-references to the saveables");
			}
			foreach (IExposable exposable2 in this.crossReferencingExposables)
			{
				if (DebugViewSettings.logMapLoad)
				{
					string str2 = "ResolvingCrossRefs ExposeData ";
					Type type2 = exposable2.GetType();
					LogSimple.Message(str2 + ((type2 != null) ? type2.ToString() : null));
				}
				try
				{
					Scribe.loader.curParent = exposable2;
					Scribe.loader.curPathRelToParent = null;
					exposable2.ExposeData();
				}
				catch (Exception ex)
				{
					string str3 = "Could not resolve cross refs: ";
					Exception ex2 = ex;
					Log.Error(str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
			this.Clear(true);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000F0C44 File Offset: 0x000EEE44
		public T TakeResolvedRef<T>(string pathRelToParent, IExposable parent) where T : ILoadReferenceable
		{
			string loadID = this.loadIDs.Take<T>(pathRelToParent, parent);
			return this.loadedObjectDirectory.ObjectWithLoadID<T>(loadID);
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000F0C6C File Offset: 0x000EEE6C
		public T TakeResolvedRef<T>(string toAppendToPathRelToParent) where T : ILoadReferenceable
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRef<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000F0CAC File Offset: 0x000EEEAC
		public List<T> TakeResolvedRefList<T>(string pathRelToParent, IExposable parent)
		{
			List<string> list = this.loadIDs.TakeList(pathRelToParent, parent);
			List<T> list2 = new List<T>();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(this.loadedObjectDirectory.ObjectWithLoadID<T>(list[i]));
				}
			}
			return list2;
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x000F0CFC File Offset: 0x000EEEFC
		public List<T> TakeResolvedRefList<T>(string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			return this.TakeResolvedRefList<T>(text, Scribe.loader.curParent);
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x000F0D3A File Offset: 0x000EEF3A
		public void Clear(bool errorIfNotEmpty)
		{
			if (errorIfNotEmpty)
			{
				this.loadIDs.ConfirmClear();
			}
			else
			{
				this.loadIDs.Clear();
			}
			this.crossReferencingExposables.Clear();
			this.loadedObjectDirectory.Clear();
		}

		// Token: 0x04001BFF RID: 7167
		private LoadedObjectDirectory loadedObjectDirectory = new LoadedObjectDirectory();

		// Token: 0x04001C00 RID: 7168
		public LoadIDsWantedBank loadIDs = new LoadIDsWantedBank();

		// Token: 0x04001C01 RID: 7169
		public List<IExposable> crossReferencingExposables = new List<IExposable>();
	}
}
