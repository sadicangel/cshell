using CommandLine;

namespace CShell;

public sealed class ShellParserException(IEnumerable<Error> errors) : Exception(FormatErrors(errors))
{
    public IEnumerable<Error> Errors { get; } = errors;

    private static string FormatErrors(IEnumerable<Error> errors) => string.Join('\n', errors.Select(FormatError));
    private static string FormatError(Error error)
    {
        switch (error.Tag)
        {
            case ErrorType.BadFormatTokenError:
                return $"Token '{((BadFormatTokenError)error).Token}' is not recognized.";
            case ErrorType.MissingValueOptionError:
                return $"Option '{((MissingValueOptionError)error).NameInfo.NameText}' has no value.";
            case ErrorType.UnknownOptionError:
                return $"Option '{((UnknownOptionError)error).Token}' is unknown.";
            case ErrorType.MissingRequiredOptionError:
                var missing = (MissingRequiredOptionError)error;
                return missing.NameInfo.Equals(NameInfo.EmptyName)
                           ? "A required value not bound to option name is missing."
                           : $"Required option '{missing.NameInfo.NameText}' is missing.";
            case ErrorType.BadFormatConversionError:
                var badFormat = (BadFormatConversionError)error;
                return badFormat.NameInfo.Equals(NameInfo.EmptyName)
                           ? "A value not bound to option name is defined with a bad format."
                           : $"Option '{badFormat.NameInfo.NameText}' is defined with a bad format.";
            case ErrorType.SequenceOutOfRangeError:
                var seqOutRange = (SequenceOutOfRangeError)error;
                return seqOutRange.NameInfo.Equals(NameInfo.EmptyName)
                           ? "A sequence value not bound to option name is defined with few items than required."
                           : $"A sequence option '{seqOutRange.NameInfo.NameText}' is defined with fewer or more items than required.";
            case ErrorType.BadVerbSelectedError:
                return $"Verb '{((BadVerbSelectedError)error).Token}' is not recognized.";
            case ErrorType.NoVerbSelectedError:
                return "No verb selected.";
            case ErrorType.RepeatedOptionError:
                return $"Option '{((RepeatedOptionError)error).NameInfo.NameText}' is defined multiple times.";
            case ErrorType.SetValueExceptionError:
                var setValueError = (SetValueExceptionError)error;
                return $"Error setting value to option '{setValueError.NameInfo.NameText}': {setValueError.Exception.Message}";
            case ErrorType.MissingGroupOptionError:
                var missingGroupOptionError = (MissingGroupOptionError)error;
                return $"At least one option from group '{missingGroupOptionError.Group}' ({string.Join(", ", missingGroupOptionError.Names.Select(n => n.NameText))} is required.";
            case ErrorType.GroupOptionAmbiguityError:
                var groupOptionAmbiguityError = (GroupOptionAmbiguityError)error;
                return $"Both SetName and Group are not allowed in option '{groupOptionAmbiguityError.Option.NameText}'";
            case ErrorType.MultipleDefaultVerbsError:
                return MultipleDefaultVerbsError.ErrorMessage;
            default:
                return "Unknown error";

        }
    }
}
