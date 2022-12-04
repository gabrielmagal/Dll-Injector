using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector
{
    internal class Messages
    {
        public String MSG_SUCESS_INJECT = "Successfully injected";
        public String MSG_FAIL_INJECT_PROCESS_INVALID = "Fail to inject, process invalid!";
        public String MSG_FAIL_ALLOC_MEMORY = "Problem to alloc memory in process!";
        public String MSG_FAIL_WRITE_DLL = "Problem to write dll path in process!";
        public String MSG_FAIL_WRITE_SHELLCODE_ADDRESS = "Problem to write shellCodeAddress!";
        public String MSG_FAIL_GET_LLA = "Problem to get address LoadLibraryA!";
        public String MSG_FAIL_GET_LLW = "Problem to get address LoadLibraryW!";
        public String MSG_FAIL_GET_LLA_OR_GPA = "Problem to get address LoadLibraryA or GetProcAddress!";
        public String MSG_FAIL_CREATE_REMOTE_THREAD = "Problem to create remote thread!";
        public String MSG_FAIL_INVALID_FILE = "Invalid file!";
        public String MSG_FAIL_INVALID_SESSION_NUMBER = "Invalid number of sections!";
    }
}
