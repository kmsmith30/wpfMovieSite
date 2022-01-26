using Caliburn.Micro;

using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;

using Kvfx.Messages;
using Kvfx.Models;

namespace Kvfx.ViewModels
{
    public class BrowseViewModel : Screen
    {
        public class Panel
        {
            public List<object> Content { get; set; }

            public string Tag { get; set; }
        }

        private IEventAggregator eventAggregator;

        private int scrollNum;

        public User CurrentUser { get; set; }

        private List<Panel> panels;
        
        public List<Panel> Panels
        {
            get => panels;
            set
            {
                if (value != null)
                {
                    panels = value;

                    NotifyOfPropertyChange(() => Panels);
                }
            }
        }

        public BrowseViewModel(IEventAggregator ea)
        {
            eventAggregator = ea;

            Init();
        }

        private void Init()
        {
            panels = new List<Panel>();

            scrollNum = 0;
        }

        public void UpdateScrollviewer()
        {

        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public void FetchContent()
        {
            //List<object> recents = User.LoadRecentlyWatched();
            List<Movie> mov = Movie.LoadAll();
            List<TvSeries> sho = TvSeries.LoadAll();


            /*
            Panels.Add(new Panel 
            { 
                Tag = "Recently Watched",

            });

            Panels.Add(new Panel
            {
                Tag = "Coen Brothers", 
                Content = new List<object> { mov[0], mov[1], mov[206], },
            });

            Panels.Add(new Panel
            {
                Tag = "David Fincher",
                Content = new List<object> { mov[2], mov[3], },
            });

            Panels.Add(new Panel
            {
                Tag = "Christopher Nolen",
                Content = new List<object> { mov[4], mov[5], mov[6], mov[7], mov[107], mov[186], },
            });

            Panels.Add(new Panel
            {
                Tag = "Judd Apatow",
                Content = new List<object> { mov[8], mov[9], },
            });

            Panels.Add(new Panel
            {
                Tag = "Stanley Kubrick",
                Content = new List<object> { mov[10], mov[11], },
            });

            Panels.Add(new Panel
            {
                Tag = "Seth Rogen",
                Content = new List<object> { mov[12], mov[13], mov[14], mov[15], mov[16], },
            });

            Panels.Add(new Panel
            {
                Tag = "Adam McKay",
                Content = new List<object> { mov[17], mov[18], mov[19], mov[20], },
            });

            Panels.Add(new Panel
            {
                Tag = "Todd Phillips",
                Content = new List<object> { mov[21], mov[22], mov[23], mov[24], mov[25], },
            });

            Panels.Add(new Panel
            {
                Tag = "Indiana Jones Collection",
                Content = new List<object> { mov[26], mov[27], mov[28], mov[29], },
            });

            Panels.Add(new Panel
            {
                Tag = "Quentin Tarantino",
                Content = new List<object> { mov[30], mov[31], mov[32], mov[33], mov[34], mov[35], },
            });

            Panels.Add(new Panel
            {
                Tag = "Harry Potter Collection",
                Content = new List<object> { mov[36], mov[37], mov[38], },
            });

            Panels.Add(new Panel
            {
                Tag = "Horrible Bosses 1+2 & We're the Millers & Cedar Rapids",
                Content = new List<object> { mov[39], mov[40], mov[41], mov[48], },
            });

            Panels.Add(new Panel
            {
                Tag = "Frank Darabont",
                Content = new List<object> { mov[42], mov[43], },
            });

            Panels.Add(new Panel 
            { 
                Tag = "Christmas Collection",
                Content = new List<object> { mov[44], mov[45], mov[46], mov[59], mov[65], },
            });

            Panels.Add(new Panel
            {
                Tag = "Spacy",
                Content = new List<object> { mov[47], mov[205], },
            });

            Panels.Add(new Panel
            {
                Tag = "Shia LaBeouf",
                Content = new List<object> { mov[49], mov[50], },
            });

            Panels.Add(new Panel
            {
                Tag = "Wayne's World 1+2",
                Content = new List<object> { mov[51], mov[52], },
            });

            Panels.Add(new Panel
            { 
                Tag = "Seth MacFarlane",
                Content = new List<object> { mov[53], mov[55], mov[54], },
            });

            Panels.Add(new Panel
            {
                Tag = "Dodgeball & American Pie & Dinner For Schmucks & Hot Rod & Lampoon's Vacation",
                Content = new List<object> { mov[56], mov[57], mov[58], mov[60], mov[64], },
            });

            Panels.Add(new Panel 
            {
                Tag = "The Lord of the Rings Collection",
                Content = new List<object> { mov[61], mov[62], mov[63], },
            });

            Panels.Add(new Panel
            {
                Tag = "Observe and Report & Project X & School of Rock",
                Content = new List<object> { mov[66], mov[67], mov[68], },
            });

            Panels.Add(new Panel
            {
                Tag = "Adam Sandler Collection",
                Content = new List<object> { mov[69], mov[70], mov[71], mov[72], mov[73], },
            });

            Panels.Add(new Panel
            {
                Tag = "Get Shorty & Lost in Translation & Weekend at Bernie's",
                Content = new List<object> { mov[74], mov[75], mov[76], },
            });

            Panels.Add(new Panel
            {
                Tag = "Lethal Weapon Collection",
                Content = new List<object> { mov[77], mov[78], mov[79], },
            });

            Panels.Add(new Panel
            {
                Tag = "Sling Blade & Stand By Me & When Harry Met Sally & Captain Phillips",
                Content = new List<object> { mov[80], mov[82], mov[83], mov[89], },
            });

            Panels.Add(new Panel
            {
                Tag = "Zoolander & Wild Hogs",
                Content = new List<object> { mov[81], mov[84], },
            });

            Panels.Add(new Panel
            {
                Tag = "Martin Scorsese",
                Content = new List<object> { mov[85], mov[86], },
            });

            Panels.Add(new Panel
            {
                Tag = "Cheaper by the Dozen & Are We There Yet?",
                Content = new List<object> { mov[87], mov[134], },
            });

            Panels.Add(new Panel
            {
                Tag = "A Bug's Life & How to Train Your Dragon",
                Content = new List<object> { mov[88], mov[92], },
            });

            Panels.Add(new Panel
            {
                Tag = "Fast Times at Ridgemont High & Groundhog Day & Vacation",
                Content = new List<object> { mov[90], mov[91], mov[97], },
            });

            Panels.Add(new Panel
            {
                Tag = "Dances with Wolves & Michael Clayton",
                Content = new List<object> { mov[93], mov[94], },
            });

            Panels.Add(new Panel
            {
                Tag = "Rocky I & II",
                Content = new List<object> { mov[95], mov[96], },
            });

            Panels.Add(new Panel
            {
                Tag = "21 & 22 Jump Street",
                Content = new List<object> { mov[98], mov[99], },
            });

            Panels.Add(new Panel
            {
                Tag = "Bushwhacked & Big Momma's House & Fletch",
                Content = new List<object> { mov[100], mov[101], mov[102], },
            });

            Panels.Add(new Panel
            {
                Tag = "Jeff Tremaine",
                Content = new List<object> { mov[103], mov[104], },
            });

            Panels.Add(new Panel
            {
                Tag = "Halloween I & II",
                Content = new List<object> { mov[105], mov[106], },
            });
            
            Panels.Add(new Panel
            {
                Tag = "Mr Woodcock & Paul Blart & Cop Out",
                Content = new List<object> { mov[108], mov[109], mov[112], },
            });

            Panels.Add(new Panel
            {
                Tag = "Major League & Days of Thunder & The Blind Side & Little Giants",
                Content = new List<object> { mov[110], mov[111], mov[113], mov[114], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Perks of Being a Wallflower",
                Content = new List<object> { mov[115] },
            });

            Panels.Add(new Panel
            {
                Tag = "Meet the Parents & Meet the Fockers",
                Content = new List<object> { mov[116], mov[117], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Watch & Couples Retreat & National Security & Revenge of the Nerds & Shallow Hal",
                Content = new List<object> { mov[118], mov[119], mov[120], mov[122], mov[126], },
            });

            Panels.Add(new Panel
            {
                Tag = "Secondhand Lions & Moneyball",
                Content = new List<object> { mov[121], mov[123], },
            });

            Panels.Add(new Panel
            {
                Tag = "Night at the Museum 1 & 2",
                Content = new List<object> { mov[124], mov[125], },
            });

            Panels.Add(new Panel
            {
                Tag = "OG Spider-Man 1-3",
                Content = new List<object> { mov[127], mov[128], mov[129], },
            });

            Panels.Add(new Panel
            {
                Tag = "Die Hard Collection",
                Content = new List<object> { mov[130], mov[131], mov[132], mov[133], },
            });

            Panels.Add(new Panel
            {
                Tag = "Gravity & National Treasure: Book of Secrets",
                Content = new List<object> { mov[135], mov[136], },
            });

            Panels.Add(new Panel
            {
                Tag = "Porky's & Semi-Pro & Stranger Than Fiction & The Goonies",
                Content = new List<object> { mov[137], mov[138], mov[139], mov[140], },
            });

            Panels.Add(new Panel
            {
                Tag = "Clint Eastwood",
                Content = new List<object> { mov[141], mov[142], mov[143], mov[144], },
            });

            Panels.Add(new Panel
            {
                Tag = "Watchmen & Batman & V For Vendetta",
                Content = new List<object> { mov[145], mov[150], mov[153], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Ringer & Silver Linings Playbook",
                Content = new List<object> { mov[146], mov[147], },
            });

            Panels.Add(new Panel
            {
                Tag = "Alien & Aliens",
                Content = new List<object> { mov[148], mov[149], },
            });

            Panels.Add(new Panel
            {
                Tag = "Coach Carter & The Sandlot & Thunderstruck",
                Content = new List<object> { mov[151], mov[157], mov[171], },
            });

            Panels.Add(new Panel
            {
                Tag = "Gremlins & Bachelor Party & The Rocker & The Great Outdoors",
                Content = new List<object> { mov[152], mov[159], mov[160], },
            });

            Panels.Add(new Panel
            {
                Tag = "Mark Wahlberg",
                Content = new List<object> { mov[154], mov[155], },
            });

            Panels.Add(new Panel
            {
                Tag = "Training Day & American Psycho",
                Content = new List<object> { mov[156], mov[158], },
            });

            Panels.Add(new Panel
            {
                Tag = "John Candy",
                Content = new List<object> { mov[161], mov[167], },
            });

            Panels.Add(new Panel
            {
                Tag = "Charlie and the Chocolate Factory & Robots",
                Content = new List<object> { mov[162], mov[181], },
            });

            Panels.Add(new Panel
            {
                Tag = "Rob Schnieder",
                Content = new List<object> { mov[163], mov[164], },
            });

            Panels.Add(new Panel
            {
                Tag = "Rush Hour 1 + 2",
                Content = new List<object> { mov[165], mov[166], },
            });

            Panels.Add(new Panel
            {
                Tag = "Burt Reynolds",
                Content = new List<object> { mov[169], mov[170], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Mummy & Terminator 2 & The Road Warrior & I Am Legend",
                Content = new List<object> { mov[172], mov[173], mov[174], mov[175], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Godfather Collection",
                Content = new List<object> { mov[176], mov[177], mov[178], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Hurt Locker & The Boy in the Striped Pajamas & Desperate Hours & Open Range",
                Content = new List<object> { mov[179], mov[180], mov[195], mov[196], },
            });

            Panels.Add(new Panel
            {
                Tag = "Taken 1 + 2",
                Content = new List<object> { mov[182], mov[183], },
            });

            Panels.Add(new Panel
            {
                Tag = "A Time to Kill & Slumdog Millionaire & Godsend & Mighty Joe Young & Dr Dolittle 2",
                Content = new List<object> { mov[184], mov[185], mov[192], mov[193], mov[199], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Internship & Daddy's Home 1 + 2 & Zombieland & Guess Who & The Wedding Ringer & Ride Along",
                Content = new List<object> { mov[187], mov[188], mov[189], mov[190], mov[191], mov[194], mov[197], },
            });

            Panels.Add(new Panel
            {
                Tag = "Wolf Creek & Tears of the Sun & Young Guns & Safe House",
                Content = new List<object> { mov[198], mov[200], mov[202], mov[203], },
            });

            Panels.Add(new Panel
            {
                Tag = "A Madea Christmas & Christmas with the Kranks", 
                Content = new List<object> { mov[201], mov[204], },
            });

            Panels.Add(new Panel
            {
                Tag = "Napoleon Dynamite & Bruno & Office Space & The Sitter & Role Models & Animal House & Bridesmaids",
                Content = new List<object> { mov[207], mov[208], mov[209], mov[211], mov[212], mov[213], mov[214], },
            });

            Panels.Add(new Panel
            {
                Tag = "Old Dogs & The Campaign & 50/50 & Easy A & Cars & The Benchwarmers",
                Content = new List<object> { mov[215], mov[216], mov[217], mov[218], mov[219], mov[220], },
            });

            Panels.Add(new Panel
            {
                Tag = "Pain and Gain & Hoosiers & Apollo 13 & War Dogs & Karate Kid & Platoon",
                Content = new List<object> { mov[221], mov[222], mov[223], mov[224], mov[225], mov[226], },
            });

            Panels.Add(new Panel
            {
                Tag = "2001: A Space Odyssey & Jumanji & Date Night & Rango & The Prestige & Road Trip",
                Content = new List<object> { mov[210], mov[227], mov[228], mov[229], mov[230], mov[231], },
            });

            Panels.Add(new Panel
            {
                Tag = "Spaceballs & Ferris Bueller & Neighbors 1+2 & It & Don't Breathe",
                Content = new List<object> { mov[232], mov[233], mov[234], mov[235], mov[236], mov[237], },
            });

            Panels.Add(new Panel
            {
                Tag = "Saw & Child's Play & Insidious 1-3 & Harry Potter 4",
                Content = new List<object> { mov[238], mov[239], mov[240], mov[241], mov[242], mov[243], },
            });

            Panels.Add(new Panel
            {
                Tag = "Hunger Games 1+2 & Austin Powers 1-3 & Evolution",
                Content = new List<object> { mov[244], mov[245], mov[246], mov[247], mov[248], mov[249], },
            });

            Panels.Add(new Panel
            {
                Tag = "Mars Attacks! & The Terminal & Forgetting Sarah Marshall & Rio & The Wolf of Wall Street",
                Content = new List<object> { mov[250], mov[251], mov[252], mov[253], mov[254], },
            });

            Panels.Add(new Panel
            {
                Tag = "Blades of Glory & Hot Tub Time Machine & Wedding Crashers & 30 Minutes or Less & Get Hard",
                Content = new List<object> { mov[255], mov[256], mov[257], mov[258], mov[259], },
            });

            Panels.Add(new Panel
            {
                Tag = "Blade Runner & District 9 & American Sniper & Get Smart & Yes Man",
                Content = new List<object> { mov[260], mov[261], mov[262], mov[263], mov[264], },
            });

            Panels.Add(new Panel
            {
                Tag = "Argo & Mr Deeds & Freaky Friday & Spy Kids 3 & Like Mike",
                Content = new List<object> { mov[265], mov[266], mov[267], mov[268], mov[269], },
            });

            Panels.Add(new Panel
            {
                Tag = "Miss March & Grudge Match & Let's Be Cops & Pixels & Accepted",
                Content = new List<object> { mov[270], mov[271], mov[272], mov[273], mov[274], },
            });

            Panels.Add(new Panel
            {
                Tag = "The Incredibles & Unaccompanied Minors & Agent Cody Banks & Remember the Titans & The DaVinci Code",
                Content = new List<object> { mov[275], mov[276], mov[277], mov[278], mov[279], },
            });
            

            Panels.Add(new Panel
            {
                Tag = "The Incredibles & Unaccompanied Minors & Agent Cody Banks & Remember the Titans & The DaVinci Code & The Mummy",
                Content = new List<object> { mov[275], mov[276], mov[277], mov[278], mov[279], mov[280], },
            });

            Panels.Add(new Panel
            {
                Tag = "American Wedding & Youth in Revolt & Up in the Air & As Good as It Gets & " +
                "A Beautiful Day in the Neighborhood & The Dictator & Jack Frost",
                Content = new List<object> { mov[281], mov[282], mov[283], mov[284], mov[285], mov[286], mov[287], },
            });
            */

            Panels.Add(new Panel
            {
                Tag = "A",
                Content = new List<object> { mov[288], mov[289], mov[290], mov[291], mov[292], },
            });

            Panels.Add(new Panel
            {
                Tag = "B",
                Content = new List<object> { mov[293], mov[294], mov[295], mov[296], mov[297], },
            });

            Panels.Add(new Panel
            {
                Tag = "C",
                Content = new List<object> { mov[298], mov[299], mov[300], mov[301], mov[302], },
            });

            Panels.Add(new Panel
            {
                Tag = "D",
                Content = new List<object> { mov[303], mov[304], mov[305], mov[306], mov[307], mov[308], },
            });

            Panels.Add(new Panel
            {
                Tag = "TV",
                Content = new List<object> { sho[0], sho[1], sho[2], sho[3], sho[4], sho[5], sho[6], },
            });
        }

        public void ContentButtonClick(object obj)
        {
            if (obj is Movie)
            {
                int id = (obj as Movie).Id;

                eventAggregator.PublishOnUIThread(new BrowseMessage
                {
                    Topic = "ToDetail",
                    ContentType = "Movie",
                    ContentId = id
                });
            }
            else if (obj is TvSeries)
            {
                int id = (obj as TvSeries).Id;

                eventAggregator.PublishOnUIThread(new BrowseMessage
                {
                    Topic = "ToDetail",
                    ContentType = "Tv",
                    ContentId = id
                });
            }
        }

        public void TryScrollbarHandle(ScrollViewer s)
        {
            scrollNum = (int)s.VerticalOffset;
        }
    }
}
