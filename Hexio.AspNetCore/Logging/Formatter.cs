using System;
using System.Collections.Generic;
using System.IO;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;

namespace Hexio.AspNetCore.Logging
{
public class Formatter : ExceptionAsObjectJsonFormatter
    {
        public Formatter() : base(renderMessage: true)
        {
        }

        protected override void WriteStructure(string typeTag, IEnumerable<LogEventProperty> properties, TextWriter output)
        {
            //Removes _typeTag field, a field called _tagTag with the class name is writting for each destructured object.
            base.WriteStructure(null, properties, output);
        }

        protected override void WriteLevel(LogEventLevel level, ref string delim, TextWriter output)
        {
            string levelText = string.Empty;
            switch (level)
            {
                case LogEventLevel.Information:
                    levelText = "Info";
                    break;
                case LogEventLevel.Warning:
                    levelText = "Warn";
                    break;
                default:
                    levelText = Enum.GetName(typeof(LogEventLevel), level);
                    break;
            }

            WriteJsonProperty("level", levelText, ref delim, output);
        }

        protected override void WriteException(Exception exception, ref string delim, TextWriter output)
        {
            output.Write(delim);
            output.Write("\"");
            output.Write("exception");
            output.Write("\":{");
            WriteExceptionTree(exception, ref delim, output, 0);
            output.Write("}");
        }

        protected new void WriteSingleException(Exception exception, ref string delim, TextWriter output, int depth)
        {
            var stackTrace = exception.StackTrace;
            var className = exception.GetType().FullName;

            WriteJsonProperty("Depth", depth, ref delim, output);
            WriteJsonProperty("ClassName", className, ref delim, output);
            WriteJsonProperty("StackTraceString", stackTrace, ref delim, output);
            WriteJsonProperty("Message", exception.Message, ref delim, output);
        }

        private void WriteExceptionTree(Exception exception, ref string delim, TextWriter output, int depth)
        {
            delim = "";
            WriteSingleException(exception, ref delim, output, depth);
            exception = exception.InnerException;
            if (exception != null)
            {
                output.Write(",");
                output.Write("\"innerException\":{");
                WriteExceptionTree(exception, ref delim, output, depth + 1);
                output.Write("}");
            }
        }
    }
}