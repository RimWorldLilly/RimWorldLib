using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000528 RID: 1320
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x060026FF RID: 9983 RVA: 0x000F2A8D File Offset: 0x000F0C8D
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.deepSavedInfo.Clear();
			this.referenced.Clear();
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000F2AB8 File Offset: 0x000F0CB8
		public void CheckForErrorsAndClear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (!Scribe.saver.savingForDebug)
			{
				foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
				{
					if (!this.deepSaved.Contains(referencedObject.loadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Object with load ID ",
							referencedObject.loadID,
							" is referenced (xml node name: ",
							referencedObject.label,
							") but is not deep-saved. This will cause errors during loading."
						}));
					}
				}
			}
			this.Clear();
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000F2B6C File Offset: 0x000F0D6C
		public void RegisterDeepSaved(object obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Registered " + ((obj != null) ? obj.ToString() : null) + ", but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (obj == null)
			{
				return;
			}
			ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
			if (loadReferenceable != null)
			{
				try
				{
					string uniqueLoadID = loadReferenceable.GetUniqueLoadID();
					if (!this.deepSaved.Add(uniqueLoadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							uniqueLoadID,
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}));
						string str;
						if (this.deepSavedInfo.TryGetValue(uniqueLoadID, out str))
						{
							Type type = loadReferenceable.GetType();
							Log.Warning(((type != null) ? type.ToString() : null) + " was already deepsaved at " + str + ".");
						}
					}
					else
					{
						this.deepSavedInfo.Add(uniqueLoadID, Scribe.saver.CurPath);
					}
				}
				catch (Exception ex)
				{
					string str2 = "Error in GetUniqueLoadID(): ";
					Exception ex2 = ex;
					Log.Error(str2 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000F2C90 File Offset: 0x000F0E90
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Registered " + ((obj != null) ? obj.ToString() : null) + ", but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (obj == null)
			{
				return;
			}
			try
			{
				this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
			}
			catch (Exception ex)
			{
				string str = "Error in GetUniqueLoadID(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04001C1D RID: 7197
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x04001C1E RID: 7198
		private Dictionary<string, string> deepSavedInfo = new Dictionary<string, string>();

		// Token: 0x04001C1F RID: 7199
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x02002773 RID: 10099
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x0600F825 RID: 63525 RVA: 0x005A026A File Offset: 0x0059E46A
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x0600F826 RID: 63526 RVA: 0x005A027A File Offset: 0x0059E47A
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x0600F827 RID: 63527 RVA: 0x005A0292 File Offset: 0x0059E492
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x0600F828 RID: 63528 RVA: 0x005A02BA File Offset: 0x0059E4BA
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x0600F829 RID: 63529 RVA: 0x005A02D3 File Offset: 0x0059E4D3
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600F82A RID: 63530 RVA: 0x005A02DD File Offset: 0x0059E4DD
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400A13E RID: 41278
			public string loadID;

			// Token: 0x0400A13F RID: 41279
			public string label;
		}
	}
}
