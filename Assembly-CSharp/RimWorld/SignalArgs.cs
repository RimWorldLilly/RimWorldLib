using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001128 RID: 4392
	public struct SignalArgs
	{
		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06006E11 RID: 28177 RVA: 0x00269525 File Offset: 0x00267725
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06006E12 RID: 28178 RVA: 0x0026952D File Offset: 0x0026772D
		public IEnumerable<NamedArgument> Args
		{
			get
			{
				SignalArgs.<get_Args>d__9 <get_Args>d__ = new SignalArgs.<get_Args>d__9(-2);
				<get_Args>d__.<>3__<>4__this = this;
				return <get_Args>d__;
			}
		}

		// Token: 0x06006E13 RID: 28179 RVA: 0x00269544 File Offset: 0x00267744
		public SignalArgs(SignalArgs args)
		{
			this.count = args.count;
			this.arg1 = args.arg1;
			this.arg2 = args.arg2;
			this.arg3 = args.arg3;
			this.arg4 = args.arg4;
			this.args = args.args;
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x00269599 File Offset: 0x00267799
		public SignalArgs(NamedArgument arg1)
		{
			this.count = 1;
			this.arg1 = arg1;
			this.arg2 = default(NamedArgument);
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x002695D4 File Offset: 0x002677D4
		public SignalArgs(NamedArgument arg1, NamedArgument arg2)
		{
			this.count = 2;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x0026960A File Offset: 0x0026780A
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.count = 3;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x0026963B File Offset: 0x0026783B
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.count = 4;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = arg4;
			this.args = null;
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x00269668 File Offset: 0x00267868
		public SignalArgs(params NamedArgument[] args)
		{
			this.count = args.Length;
			if (args.Length > 4)
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
				this.args = new NamedArgument[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					this.args[i] = args[i];
				}
				return;
			}
			if (args.Length == 1)
			{
				this.arg1 = args[0];
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 2)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 3)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 4)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = args[3];
			}
			else
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.args = null;
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x0026980C File Offset: 0x00267A0C
		public bool TryGetArg(int index, out NamedArgument arg)
		{
			if (index < 0 || index >= this.count)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				arg = this.args[index];
			}
			else if (index == 0)
			{
				arg = this.arg1;
			}
			else if (index == 1)
			{
				arg = this.arg2;
			}
			else if (index == 2)
			{
				arg = this.arg3;
			}
			else
			{
				arg = this.arg4;
			}
			return true;
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x00269890 File Offset: 0x00267A90
		public bool TryGetArg(string name, out NamedArgument arg)
		{
			if (this.count == 0)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				for (int i = 0; i < this.args.Length; i++)
				{
					if (this.args[i].label == name)
					{
						arg = this.args[i];
						return true;
					}
				}
			}
			else
			{
				if (this.count >= 1 && this.arg1.label == name)
				{
					arg = this.arg1;
					return true;
				}
				if (this.count >= 2 && this.arg2.label == name)
				{
					arg = this.arg2;
					return true;
				}
				if (this.count >= 3 && this.arg3.label == name)
				{
					arg = this.arg3;
					return true;
				}
				if (this.count >= 4 && this.arg4.label == name)
				{
					arg = this.arg4;
					return true;
				}
			}
			arg = default(NamedArgument);
			return false;
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x002699AC File Offset: 0x00267BAC
		public bool TryGetArg<T>(int index, out T arg)
		{
			NamedArgument namedArgument;
			if (this.TryGetArg(index, out namedArgument))
			{
				object arg2 = namedArgument.arg;
				if (arg2 is T)
				{
					T t = (T)((object)arg2);
					arg = t;
					return true;
				}
			}
			arg = default(T);
			return false;
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x002699F0 File Offset: 0x00267BF0
		public bool TryGetArg<T>(string name, out T arg)
		{
			NamedArgument namedArgument;
			if (!this.TryGetArg(name, out namedArgument) || !(namedArgument.arg is T))
			{
				arg = default(T);
				return false;
			}
			arg = (T)((object)namedArgument.arg);
			return true;
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x00269A30 File Offset: 0x00267C30
		public NamedArgument GetArg(int index)
		{
			NamedArgument result;
			if (this.TryGetArg(index, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x00269A54 File Offset: 0x00267C54
		public NamedArgument GetArg(string name)
		{
			NamedArgument result;
			if (this.TryGetArg(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name);
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x00269A80 File Offset: 0x00267C80
		public T GetArg<T>(string name)
		{
			T result;
			if (this.TryGetArg<T>(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x00269AC0 File Offset: 0x00267CC0
		public TaggedString GetFormattedText(TaggedString text)
		{
			if (this.count == 0)
			{
				return text.Formatted(Array.Empty<NamedArgument>());
			}
			if (this.args != null)
			{
				return text.Formatted(this.args);
			}
			if (this.count == 1)
			{
				return text.Formatted(this.arg1);
			}
			if (this.count == 2)
			{
				return text.Formatted(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return text.Formatted(this.arg1, this.arg2, this.arg3);
			}
			return text.Formatted(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x00269B68 File Offset: 0x00267D68
		public TaggedString GetTranslatedText(string textKey)
		{
			if (this.count == 0)
			{
				return textKey.Translate();
			}
			if (this.args != null)
			{
				return textKey.Translate(this.args);
			}
			if (this.count == 1)
			{
				return textKey.Translate(this.arg1);
			}
			if (this.count == 2)
			{
				return textKey.Translate(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return textKey.Translate(this.arg1, this.arg2, this.arg3);
			}
			return textKey.Translate(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x06006E22 RID: 28194 RVA: 0x00269C0C File Offset: 0x00267E0C
		public void Add(NamedArgument arg)
		{
			if (this.args != null)
			{
				NamedArgument[] array = new NamedArgument[this.args.Length + 1];
				for (int i = 0; i < this.args.Length; i++)
				{
					array[i] = this.args[i];
				}
				array[array.Length - 1] = arg;
				this.args = array;
				this.count = this.args.Length;
				return;
			}
			if (this.count == 0)
			{
				this.arg1 = arg;
			}
			else if (this.count == 1)
			{
				this.arg2 = arg;
			}
			else if (this.count == 2)
			{
				this.arg3 = arg;
			}
			else if (this.count == 3)
			{
				this.arg4 = arg;
			}
			else
			{
				this.args = new NamedArgument[5];
				this.args[0] = this.arg1;
				this.args[1] = this.arg2;
				this.args[2] = this.arg3;
				this.args[3] = this.arg4;
				this.args[4] = arg;
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.count++;
		}

		// Token: 0x06006E23 RID: 28195 RVA: 0x00269D6B File Offset: 0x00267F6B
		public void Add(NamedArgument arg1, NamedArgument arg2)
		{
			this.Add(arg1);
			this.Add(arg2);
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x00269D7B File Offset: 0x00267F7B
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x00269D92 File Offset: 0x00267F92
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
			this.Add(arg4);
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x00269DB4 File Offset: 0x00267FB4
		public void Add(params NamedArgument[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				this.Add(args[i]);
			}
		}

		// Token: 0x06006E27 RID: 28199 RVA: 0x00269DDC File Offset: 0x00267FDC
		public void Add(SignalArgs args)
		{
			if (args.count == 0)
			{
				return;
			}
			if (args.args != null)
			{
				for (int i = 0; i < args.args.Length; i++)
				{
					this.Add(args.args[i]);
				}
				return;
			}
			if (args.count >= 1)
			{
				this.Add(args.arg1);
			}
			if (args.count >= 2)
			{
				this.Add(args.arg2);
			}
			if (args.count >= 3)
			{
				this.Add(args.arg3);
			}
			if (args.count >= 4)
			{
				this.Add(args.arg4);
			}
		}

		// Token: 0x04004656 RID: 18006
		private int count;

		// Token: 0x04004657 RID: 18007
		private NamedArgument arg1;

		// Token: 0x04004658 RID: 18008
		private NamedArgument arg2;

		// Token: 0x04004659 RID: 18009
		private NamedArgument arg3;

		// Token: 0x0400465A RID: 18010
		private NamedArgument arg4;

		// Token: 0x0400465B RID: 18011
		private NamedArgument[] args;
	}
}
