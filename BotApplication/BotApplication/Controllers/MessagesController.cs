using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace BotApplication
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<Message> Post([FromBody] Message message)
        {
            return await Conversation.SendAsync(message, () => new DocAssistDialog());
        }
    }

    [LuisModel("630f67a3-cbda-4594-a9b3-e44a65465e01", "1abc0a0e7ecb472c874824b9926117bb")]
    [Serializable]
    public class DocAssistDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("SearchLibraryForFunctionInClass")]
        public async Task SearchLibraryForFunctionInClass(IDialogContext context, LuisResult result)
        {
            var message = "Could not find any documentation. Sorry try a different search.";

            if (result.Entities.First(i => i.Type == "library").Entity == "lodash")
            {
                if (result.Entities.First(i => i.Type == "class").Entity == "array")
                {
                    if (result.Entities.First(i => i.Type == "function").Entity == "chunk")
                    {
                        message = @"
# _.chunk(array, [size=1])
Creates an array of elements split into groups the length of size. If array can’t be split evenly, the final chunk will be the remaining elements.
## Since
3.0.0
## Arguments
*array* (**Array**): The array to process.

*size=1* (**number**): *optional*, The length of each chunk
## Returns
(**Array**): Returns the new array containing chunks.
## Example
`_.chunk(['a', 'b', 'c', 'd'], 2);`

`// → [['a', 'b'], ['c', 'd']]`

`_.chunk(['a', 'b', 'c', 'd'], 3);`

`// → [['a', 'b', 'c'], ['d']]`";

                    }
                }
            }

            await context.PostAsync(message);

            context.Wait(MessageReceived);
        }
    }

}