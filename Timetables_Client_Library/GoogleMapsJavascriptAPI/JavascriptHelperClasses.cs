using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Client
{
	public class JavascriptString
	{
		private string content;
		public override string ToString() => content;
		public JavascriptString(string content) => this.content = "'" + content + "'";
	}
	public class JavascriptArray<T> : List<T>
	{
		public JavascriptArray() : base() { }
		public JavascriptArray(int capacity) : base(capacity) { }
		public JavascriptArray(IEnumerable<T> collection) : base(collection) { }
		public override string ToString() => "[ " + string.Join(", ", this) + " ]";
	}
	public class JavascriptVariable<T>
	{
		public string Name { get; }
		public T Content { get; set; }
		public static implicit operator T (JavascriptVariable<T> var) => var.Content; 
		public JavascriptVariable(string name, T content)
		{
			Name = name;
			Content = content;
		}
		public override string ToString() => Name;
		public string VariableAssignment() => $"var { Name } = { Content.ToString() };";
		public string VariableReassignment() => $"{ Name } = { Content.ToString() };";
		public string Call(JavascriptFunction.Call fn) => $"{ Name }.{ fn.ToString() }";
	}
	public class JavascriptAnonymousObject
	{
		public List<KeyValuePair<string, object>> ConstructorParameters { get; set; }
		public JavascriptAnonymousObject(params KeyValuePair<string, object>[] parameters) => ConstructorParameters = new List<KeyValuePair<string, object>>(parameters);
		public override string ToString() => "{ " + string.Join(", ", ConstructorParameters.Select(p => p.Key + ": " + p.Value)) + " }";

	}
	public class JavascriptObject
	{
		public string ClassName { get; }
		public List<object> ConstructorParameters { get; }
		public JavascriptObject(string name, params object[] parameters)
		{
			ClassName = name;
			ConstructorParameters = new List<object>(parameters);
		}

		public override string ToString() => $"new { ClassName } ({ string.Join(", ", ConstructorParameters) })";
	}
	public abstract class JavascriptControlStructures
	{
		public class For : JavascriptControlStructures
		{
			public JavascriptVariable<uint> ControlVariable { get; }
			public uint UpperBound { get; }
			public For(JavascriptVariable<uint> var, uint bound)
			{
				ControlVariable = var;
				UpperBound = bound;
			}
			public override string ToString() => $"for({ ControlVariable.VariableAssignment() } { ControlVariable } < { UpperBound }; { ControlVariable }++) " + base.ToString();
		}
		public List<Func<string>> Instructions { get; } = new List<Func<string>>();
		public void AddInstruction(Func<string> action) => Instructions.Add(action);
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("{");
			foreach (var instruction in Instructions) sb.AppendLine(instruction());
			sb.Append("}");
			return sb.ToString();
		}
	}
	public abstract class JavascriptFunction
	{
		public class Call : JavascriptFunction
		{
			public Call(string name, params object[] parameters)
			{
				FunctionName = name;
				FunctionParameters = new List<object>(parameters);
			}
			public override string ToString() => base.ToString();
		}
		public class Definition : JavascriptFunction
		{
			public List<Func<string>> Instructions { get; } = new List<Func<string>>();
			public Definition(string name, params object[] parameters)
			{
				FunctionName = name;
				FunctionParameters = new List<object>(parameters);
			}
			public void AddInstruction(Func<string> action) => Instructions.Add(action);
			public static implicit operator Call (Definition fn) => new Call(fn.FunctionName, fn.FunctionParameters.ToArray());
			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine($"function { base.ToString() } {{");
				foreach (var instruction in Instructions) sb.AppendLine(instruction());
				sb.Append("}");
				return sb.ToString();
			}
		}
		public class Anonymous : Definition
		{
			public Anonymous(params object[] parameters) : base(string.Empty, parameters) { }
		}
		public string FunctionName { get; protected set; }
		public List<object> FunctionParameters { get; protected set; }
		public override string ToString() => $"{ FunctionName }({ string.Join(", ", FunctionParameters) })";
	}
}
