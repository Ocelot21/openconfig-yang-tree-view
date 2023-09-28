using openconfig_yang_tree_view.Core;
using openconfig_yang_tree_view.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class FilesViewModel : BaseViewModel
    {
        private IFilesService _filesService;

        public IFilesService FilesService
        {
            get => _filesService;
            set
            {
                _filesService = value;
                OnPropertyChanged(nameof(FilesService));
            }
        }

        private string _selectedFolderPath;

        public string SelectedFolderPath 
        { 
            get { return _selectedFolderPath;  } 
            set
            {
                if (_selectedFolderPath != value)
                {
                    _selectedFolderPath = value;
                    OnPropertyChanged(nameof(SelectedFolderPath));
                }
            }
        }

        private string _parsedFiles;
        public string ParsedFiles
        {
            get { return _parsedFiles; }
            set
            {
                if (_parsedFiles != value)
                {
                    _parsedFiles = value;
                    OnPropertyChanged(nameof(ParsedFiles));
                }
            }
        }

        private string _missingFiles;
        public string MissingFiles
        {
            get { return _missingFiles; }
            set
            {
                if (_missingFiles != value)
                {
                    _missingFiles = value;
                    OnPropertyChanged(nameof(MissingFiles));
                }
            }
        }

        public RelayCommand SelectFolderCommand { get; set; }
        public RelayCommand ParseFromFolderCommand { get; set; }

        public FilesViewModel(IFilesService filesService)
        {
            _filesService = filesService;
            SelectFolderCommand = new RelayCommand(SetFolderPath, obj => true);
            ParseFromFolderCommand = new RelayCommand(ParseFromFolder, obj => true);
            SelectedFolderPath = string.Empty;
            ParsedFiles = string.Empty;
            MissingFiles = string.Empty;
        }

        private void SetFolderPath(object parameter)
        {
            string folderPath = _filesService.SelectFolder();
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                SelectedFolderPath = folderPath;
            } 
        }

        private void ParseFromFolder(object parameter)
        {
            _filesService.ParseFromFolder(SelectedFolderPath);
            ParsedFiles = _filesService.GetParsedFiles();
            MissingFiles = _filesService.GetMissingFiles();
        }
    }
}
