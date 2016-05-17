using GoFish.Services.Requests;
using GoFish.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish.Services
{
    public class DataService
    {
        private const string CARD_API = "http://deckofcardsapi.com/api";
        public string NewDecks(int numberOfDecks)
        {
            String Uri = CARD_API + "/deck/new/shuffle/?deck_count=" + numberOfDecks;
            Uri serviceUri = new Uri(Uri);
            ServiceManager svcMgr = new ServiceManager(serviceUri);
            var response = svcMgr.CallService<DeckResponse>();
            return response.DeckID;
        }

        public DrawResponse Draw(DrawRequest drawReq) {
            String Uri = CARD_API + "/deck/" + drawReq.DeckID + "/draw/?count=" + drawReq.NumberOfCards;
            Uri serviceUri = new Uri(Uri);
            ServiceManager svcMgr = new ServiceManager(serviceUri);
            return svcMgr.CallService<DrawResponse>();
        }
    }
}
