using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000522 RID: 1314
	public class LoadIDsWantedBank
	{
		// Token: 0x060026DB RID: 9947 RVA: 0x000F1298 File Offset: 0x000EF498
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> keyValuePair in this.idsRead)
					{
						StringBuilder stringBuilder2 = stringBuilder;
						string[] array = new string[8];
						array[0] = "  ";
						array[1] = keyValuePair.Value.targetLoadID.ToStringSafe<string>();
						array[2] = " of type ";
						int num = 3;
						Type targetType = keyValuePair.Value.targetType;
						array[num] = ((targetType != null) ? targetType.ToString() : null);
						array[4] = ". pathRelToParent=";
						array[5] = keyValuePair.Value.pathRelToParent;
						array[6] = ", parent=";
						array[7] = keyValuePair.Value.parent.ToStringSafe<IExposable>();
						stringBuilder2.AppendLine(string.Concat(array));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> keyValuePair2 in this.idListsRead)
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"  List with ",
							((keyValuePair2.Value.targetLoadIDs != null) ? keyValuePair2.Value.targetLoadIDs.Count : 0).ToString(),
							" elements. pathRelToParent=",
							keyValuePair2.Value.pathRelToParent,
							", parent=",
							keyValuePair2.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines());
			}
			this.Clear();
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000F14AC File Offset: 0x000EF6AC
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000F14C4 File Offset: 0x000EF6C4
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			if (this.idsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to register the same load ID twice: ",
					targetLoadID,
					", pathRelToParent=",
					pathRelToParent,
					", parent=",
					parent.ToStringSafe<IExposable>()
				}));
				return;
			}
			LoadIDsWantedBank.IdRecord value = new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent);
			this.idsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000F1544 File Offset: 0x000EF744
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000F1584 File Offset: 0x000EF784
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			if (this.idListsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
				return;
			}
			LoadIDsWantedBank.IdListRecord value = new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent);
			this.idListsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x000F15E0 File Offset: 0x000EF7E0
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x000F1620 File Offset: 0x000EF820
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdRecord idRecord;
			if (this.idsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idRecord))
			{
				string targetLoadID = idRecord.targetLoadID;
				if (typeof(T) != idRecord.targetType)
				{
					string[] array = new string[8];
					array[0] = "Trying to get load ID of object of type ";
					int num = 1;
					Type typeFromHandle = typeof(T);
					array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
					array[2] = ", but it was registered as ";
					int num2 = 3;
					Type targetType = idRecord.targetType;
					array[num2] = ((targetType != null) ? targetType.ToString() : null);
					array[4] = ". pathRelToParent=";
					array[5] = pathRelToParent;
					array[6] = ", parent=";
					array[7] = parent.ToStringSafe<IExposable>();
					Log.Error(string.Concat(array));
				}
				this.idsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadID;
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return null;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000F1700 File Offset: 0x000EF900
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdListRecord idListRecord;
			if (this.idListsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idListRecord))
			{
				List<string> targetLoadIDs = idListRecord.targetLoadIDs;
				this.idListsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadIDs;
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return new List<string>();
		}

		// Token: 0x04001C04 RID: 7172
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> idsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord>();

		// Token: 0x04001C05 RID: 7173
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> idListsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord>();

		// Token: 0x02002771 RID: 10097
		private struct IdRecord
		{
			// Token: 0x0600F823 RID: 63523 RVA: 0x005A0234 File Offset: 0x0059E434
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x0400A137 RID: 41271
			public string targetLoadID;

			// Token: 0x0400A138 RID: 41272
			public Type targetType;

			// Token: 0x0400A139 RID: 41273
			public string pathRelToParent;

			// Token: 0x0400A13A RID: 41274
			public IExposable parent;
		}

		// Token: 0x02002772 RID: 10098
		private struct IdListRecord
		{
			// Token: 0x0600F824 RID: 63524 RVA: 0x005A0253 File Offset: 0x0059E453
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x0400A13B RID: 41275
			public List<string> targetLoadIDs;

			// Token: 0x0400A13C RID: 41276
			public string pathRelToParent;

			// Token: 0x0400A13D RID: 41277
			public IExposable parent;
		}
	}
}
