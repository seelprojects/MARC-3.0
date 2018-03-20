# Mobile Application Review Classifier (MARC) 3.0


MARC 3.0 is the third release of our [Mobile Application Review Classifier MARC](https://github.com/seelprojects/MARC). MARC 3.0 provides functionality for automatically classifying and summarizing user reviews on mobile application stores, an enhanced data classification engine, and a new GUI.

![ScreenShot](https://raw.githubusercontent.com/seelprojects/MARC-3.0/master/MARC%203.0/Images/1.PNG)

# Summarization

MARC 3.0 provides multiple data summarization algorithms that can be used to generate concise and comprehensive summaries of user reviews. These algorithms include Hybrid TF, Hybrid TFIDF, SumBasic, and LexRank.
![ScreenShot](https://raw.githubusercontent.com/seelprojects/MARC-3.0/master/MARC%203.0/Images/3.PNG)

# Non-Functional Requirements Classification
MARC 3.0 adds a new functionality to extract Non-Functional Requirements (NFRs) from user reviews. The reviews are classified into 4 major NFR classes, including Dependability, Performance, Supportability and Usability. This classification is done in two steps: 1) The NFR concerns first isolated using binary classification. 2) Dictionary Matching is used to classify user reviews into 4 NFR classes. Users have the freedom to select their own training dataset and dictionary list. Please make sure that the custom training file and indicator term list files match the format provided in the source code. The custom files must be added to C:/Users/*name*/AppData/Roaming/MARC 3.0/InputData/TrainingDataset" folder.
![ScreenShot](https://raw.githubusercontent.com/seelprojects/MARC-3.0/master/MARC%203.0/Images/4.PNG)

# Resources
1- [Weka: Data Mining Software in Java](http://www.cs.waikato.ac.nz/ml/weka/)

2- [LexRank](https://github.com/ericouyang/summarizer51-java)

3- [Word Cloud](https://www.codeproject.com/Articles/224231/Word-Cloud-Tag-Cloud-Generator-Control-for-NET-Win)

4- [Framenet Interface](http://www.cs.cmu.edu/~ark/SEMAFOR/)

# Installation

MARC requires .Net 4.5.2 and Java 1.8 to run. MARC can be installed by running the installer from the directory: [MARC Installer -> Debug -> MARC 3.0 Installer.msi](https://github.com/seelprojects/MARC-3.0/tree/master/MARC%203.0%20Installer/Debug)

MARC provides default training datasets (BOF Dataset.arff and BOW Dataset.arff) in the local app data installation directory (C:\Users\{Username}\AppData\Roaming\MARC 3.0\InputData). You can either edit this training dataset or use one of your own. However, please make sure that the training dataset you use follows the same format as the default training dataset.

# Modification

In order to open and modify the C# source project you will need [Visual Studio 2015, FreeCommunity Edition](https://www.visualstudio.com/vs/community/) .Net 4.5.2. Once you have loaded the project open MARC 3.0.sln in src directory in Visual Studio and select MARC as the startup project. You may also have to link references from the project directory.

# License

Please refer to the file LICENSE.md for license information.

# Disclaimer

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
