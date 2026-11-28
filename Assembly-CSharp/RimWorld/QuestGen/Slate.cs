using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C7 RID: 8903
	public class Slate
	{
		// Token: 0x17002632 RID: 9778
		// (get) Token: 0x0600DB93 RID: 56211 RVA: 0x0051D501 File Offset: 0x0051B701
		public string CurrentPrefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x0600DB94 RID: 56212 RVA: 0x0051D50C File Offset: 0x0051B70C
		public T Get<T>(string name, T defaultValue = default(T), bool isAbsoluteName = false)
		{
			T result;
			if (this.TryGet<T>(name, out result, isAbsoluteName))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x0600DB95 RID: 56213 RVA: 0x0051D528 File Offset: 0x0051B728
		public bool TryGet<T>(string name, out T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				var = default(T);
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			object obj;
			if (!this.vars.TryGetValue(name, out obj))
			{
				var = default(T);
				return false;
			}
			if (obj == null)
			{
				var = default(T);
				return true;
			}
			if (obj is T)
			{
				var = (T)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<T>(obj))
			{
				var = ConvertHelper.Convert<T>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert slate variable \"",
				name,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(T).Name
			}));
			var = default(T);
			return false;
		}

		// Token: 0x0600DB96 RID: 56214 RVA: 0x0051D628 File Offset: 0x0051B828
		public void Set<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				Log.Error("Tried to set a variable with null name. var=" + var.ToStringSafe<T>());
				return;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			ISlateRef slateRef = var as ISlateRef;
			if (slateRef != null)
			{
				object value;
				slateRef.TryGetConvertedValue<object>(this, out value);
				this.vars[name] = value;
				return;
			}
			this.vars[name] = var;
		}

		// Token: 0x0600DB97 RID: 56215 RVA: 0x0051D6B7 File Offset: 0x0051B8B7
		public void SetIfNone<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (this.Exists(name, isAbsoluteName))
			{
				return;
			}
			this.Set<T>(name, var, isAbsoluteName);
		}

		// Token: 0x0600DB98 RID: 56216 RVA: 0x0051D6D0 File Offset: 0x0051B8D0
		public bool Remove(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			return this.vars.Remove(name);
		}

		// Token: 0x0600DB99 RID: 56217 RVA: 0x0051D720 File Offset: 0x0051B920
		public bool Exists(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			return this.vars.ContainsKey(name);
		}

		// Token: 0x0600DB9A RID: 56218 RVA: 0x0051D780 File Offset: 0x0051B980
		private string TryResolveFirstAvailableName(string nameWithPrefix)
		{
			if (nameWithPrefix == null)
			{
				return null;
			}
			nameWithPrefix = QuestGenUtility.NormalizeVarPath(nameWithPrefix);
			if (this.vars.ContainsKey(nameWithPrefix))
			{
				return nameWithPrefix;
			}
			int num = nameWithPrefix.LastIndexOf('/');
			if (num < 0)
			{
				return nameWithPrefix;
			}
			string text = nameWithPrefix.Substring(num + 1);
			string text2 = nameWithPrefix.Substring(0, num);
			string text3;
			for (;;)
			{
				text3 = text;
				if (!text2.NullOrEmpty())
				{
					text3 = text2 + "/" + text3;
				}
				if (this.vars.ContainsKey(text3))
				{
					break;
				}
				if (text2.NullOrEmpty())
				{
					return nameWithPrefix;
				}
				int num2 = text2.LastIndexOf('/');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				else
				{
					text2 = "";
				}
			}
			return text3;
		}

		// Token: 0x0600DB9B RID: 56219 RVA: 0x0051D820 File Offset: 0x0051BA20
		public void PushPrefix(string newPrefix, bool allowNonPrefixedLookup = false)
		{
			if (newPrefix.NullOrEmpty())
			{
				Log.Error("Tried to push a null prefix.");
				newPrefix = "unnamed";
			}
			if (!this.prefix.NullOrEmpty())
			{
				this.prefix += "/";
			}
			this.prefix += newPrefix;
			this.prevAllowNonPrefixedLookupStack.Push(this.allowNonPrefixedLookup);
			if (allowNonPrefixedLookup)
			{
				this.allowNonPrefixedLookup = true;
			}
		}

		// Token: 0x0600DB9C RID: 56220 RVA: 0x0051D898 File Offset: 0x0051BA98
		public void PopPrefix()
		{
			int num = this.prefix.LastIndexOf('/');
			if (num >= 0)
			{
				this.prefix = this.prefix.Substring(0, num);
			}
			else
			{
				this.prefix = "";
			}
			if (this.prevAllowNonPrefixedLookupStack.Count != 0)
			{
				this.allowNonPrefixedLookup = this.prevAllowNonPrefixedLookupStack.Pop();
			}
		}

		// Token: 0x0600DB9D RID: 56221 RVA: 0x0051D8F8 File Offset: 0x0051BAF8
		public Slate.VarRestoreInfo GetRestoreInfo(string name)
		{
			bool flag = this.allowNonPrefixedLookup;
			this.allowNonPrefixedLookup = false;
			Slate.VarRestoreInfo result;
			try
			{
				object value;
				bool exists = this.TryGet<object>(name, out value, false);
				result = new Slate.VarRestoreInfo(name, exists, value);
			}
			finally
			{
				this.allowNonPrefixedLookup = flag;
			}
			return result;
		}

		// Token: 0x0600DB9E RID: 56222 RVA: 0x0051D944 File Offset: 0x0051BB44
		public void Restore(Slate.VarRestoreInfo varRestoreInfo)
		{
			if (varRestoreInfo.exists)
			{
				this.Set<object>(varRestoreInfo.name, varRestoreInfo.value, false);
				return;
			}
			this.Remove(varRestoreInfo.name, false);
		}

		// Token: 0x0600DB9F RID: 56223 RVA: 0x0051D970 File Offset: 0x0051BB70
		public void SetAll(Slate otherSlate)
		{
			this.vars.Clear();
			foreach (KeyValuePair<string, object> keyValuePair in otherSlate.vars)
			{
				this.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x0600DBA0 RID: 56224 RVA: 0x0051D9E0 File Offset: 0x0051BBE0
		public void Reset()
		{
			this.vars.Clear();
		}

		// Token: 0x0600DBA1 RID: 56225 RVA: 0x0051D9F0 File Offset: 0x0051BBF0
		public Slate DeepCopy()
		{
			Slate slate = new Slate();
			slate.prefix = this.prefix;
			foreach (KeyValuePair<string, object> keyValuePair in this.vars)
			{
				slate.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return slate;
		}

		// Token: 0x0600DBA2 RID: 56226 RVA: 0x0051DA68 File Offset: 0x0051BC68
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in from x in this.vars
			orderby x.Key
			select x)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				string str = (keyValuePair.Value is IEnumerable && !(keyValuePair.Value is string)) ? ((IEnumerable)keyValuePair.Value).ToStringSafeEnumerable() : keyValuePair.Value.ToStringSafe<object>();
				stringBuilder.Append(keyValuePair.Key + "=" + str);
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("(none)");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04008CC4 RID: 36036
		private Dictionary<string, object> vars = new Dictionary<string, object>();

		// Token: 0x04008CC5 RID: 36037
		private string prefix = "";

		// Token: 0x04008CC6 RID: 36038
		private bool allowNonPrefixedLookup;

		// Token: 0x04008CC7 RID: 36039
		private Stack<bool> prevAllowNonPrefixedLookupStack = new Stack<bool>();

		// Token: 0x04008CC8 RID: 36040
		public const char Separator = '/';

		// Token: 0x02003DC7 RID: 15815
		public struct VarRestoreInfo
		{
			// Token: 0x06015EA0 RID: 89760 RVA: 0x006D9209 File Offset: 0x006D7409
			public VarRestoreInfo(string name, bool exists, object value)
			{
				this.name = name;
				this.exists = exists;
				this.value = value;
			}

			// Token: 0x0400F37A RID: 62330
			public string name;

			// Token: 0x0400F37B RID: 62331
			public bool exists;

			// Token: 0x0400F37C RID: 62332
			public object value;
		}
	}
}
