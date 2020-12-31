# series_watcher_api
an api that has a list of tv shows that I am currently watching and their details (season and episode)  
and triggers me by sending a whatsapp message when a new episode is out.

1. I have a list of tv shows that i am following saved in db with the details. 
2. I use TheMovieDb (TMDB) api to get the details of the tv show and the list of episode. 
3. I generate a report based on the info I get from TMDB. The report is focused on episodes that were 
out in the last week or in the coming week from the day the api is ran.
4. Then I use twilio to send this report to my whatsapp number which is preconfigured on the twilio sandbox environment. 
5. The added bonus is using the TMDB api to discover some new tv series (randomly chosen) then sent as part of the report to my whatsapp number. 
