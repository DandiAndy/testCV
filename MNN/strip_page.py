import urllib
import os
from parsel import Selector

class strip_page():
    
    # Start stripping the page.
    def start(self, filename, directory_name):
        print("Starting...")
        file = self.load_file(filename)
        url_list = self.parse(file)
        self.close_file(file)
        self.download_imgs(directory_name, url_list)

    # Load the html file into the program and return it.
    def load_file(self, filename):
        print("Load the entered filename...")
        file = open(filename+".html", "r")
        return file

    # Close the html file after the we have the list of url.
    def close_file(self, file):
        print("Closing the file...")
        file.close()
  
    # Parse throught the html file to find the right thumbnail urls. Add to a list.
    def parse(self, file):
        print("Parse the html to find the thumbnail url...")
        url_list = []
        text = file.read()
        selector = Selector(text=text)
        for e in selector.css('img'):
            check = e.xpath('@class').extract()
            if(len(check) > 0):
                if(check[0] == 'thumbnailImage'):
                    img_url = e.xpath('@src').extract()
                    url_list = url_list + (img_url)

        print("# of urls: " + str(len(url_list)))            
        return url_list

    # Download all of the images to the named local directory. If directory doesn't exist, add it.
    def download_imgs(self, directory_name, url_list):
        print("Download all the images from the url list...")
        count=1
        if not os.path.exists(directory_name):
            os.makedirs(directory_name)
        for url in url_list:
            urllib.request.urlretrieve(url, "./"+directory_name+"/"+directory_name+str(count)+".jpg")
            count = count + 1

# Run the script. Input a html file name to strip and directory name to save images to.
if __name__ == "__main__":
    filename = input("Enter a html filename (without filetype tag): ")
    dir_name = input("Enter in a directory name (in current directory): ")
    sp = strip_page()
    sp.start(filename, dir_name)