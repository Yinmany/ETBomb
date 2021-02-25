using UnityEditor;
using UnityEditor.Compilation;

namespace Editor
{
    [InitializeOnLoad]
    public class ViewModelEditor
    {
        static ViewModelEditor()
        {
            
            CompilationPipeline.assemblyCompilationFinished += (s, messages) =>
            {
                
            };
        }
    }
}