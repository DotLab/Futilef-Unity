using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script to generate a C# file containing a static class with the sizes of the value types
/// in the runtime project. 
/// </summary>
/// <author>JacksonDunstan.com/articles/3921</author>
/// <license>MIT</license>
public class GenerateTypeSizes {
	[MenuItem("Code Generators/Type Sizes")]
	public static void Generate() {
		// Find the assembly with the runtime code
		Assembly runtimeAssembly = null;
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			if (assembly.FullName.StartsWith("Assembly-CSharp,")) {
				runtimeAssembly = assembly;
				break;
			}
		}
		if (runtimeAssembly == null) {
			Debug.LogError("Couldn't find runtime assembly. No generating code.");
			return;
		}

		// Overwrite the output file
		const string outputClassName = "TypeSizes";
		string path = Path.Combine(Application.dataPath, outputClassName) + ".txt";
		using (FileStream stream = File.OpenWrite(path)) {
			// Clear the existing file
			stream.SetLength(0);
			stream.Flush();

			// Write the sizes
			// Also make a list of constant types
			StreamWriter writer = new StreamWriter(stream);
			writer.Write("public static class ");
			writer.Write(outputClassName);
			writer.Write(" {\n");
			foreach (Type type in runtimeAssembly.GetTypes()) {
				// Only output value types
				// Skip the output type itself
				// Skip enums
				if (type.IsValueType && !type.IsEnum && type.Name != outputClassName && type.FullName.IndexOf('<') == -1) {
					// Make sure it doesn't have any non-value field types except pointers
					bool hasPointers = false;
					foreach (FieldInfo field in type.GetFields()) {
						bool isPointer = field.FieldType.IsPointer;
						if (isPointer) {
							hasPointers = true;
						}
						if (!field.FieldType.IsValueType && !isPointer) {
							Debug.LogWarningFormat(
								"{0}.{1} is a {2}, which is not a value type. Not outputting size.",
								type.Name,
								field.Name,
								field.FieldType);
							goto continueOuterLoop;
						}
					}

					writer.Write("\tpublic ");

					if (hasPointers) {
						// The size of a pointer differs by CPU architecture (e.g. 32-bit vs. 64-bit)
						// We're forced to use sizeof(MyStruct) to determine it at runtime
						writer.Write("static readonly uint ");
						writer.Write(type.FullName);
						writer.Write(" = (uint)sizeof(");
						writer.Write(type.FullName);
						writer.Write(')');
					} else {
						// The size of structs with no pointers and only value types can be
						// determined by Marshal.SizeOf.
						writer.Write("const uint ");
						writer.Write(type.FullName);
						writer.Write(" = ");
						writer.Write(Marshal.SizeOf(type));
					}

					writer.Write(";\n");
				}
				continueOuterLoop:
				;
			}

			// End the file
			writer.Write("}\n");
			writer.Flush();
		}

		// Done
		AssetDatabase.Refresh();
		Debug.LogFormat("Successfully generated {0}", outputClassName);
	}
}