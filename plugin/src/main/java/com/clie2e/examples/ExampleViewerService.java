package com.clie2e.examples;

import com.intellij.openapi.fileEditor.FileEditorManager;
import com.intellij.openapi.project.Project;
import com.intellij.openapi.vfs.VirtualFile;
import com.intellij.testFramework.LightVirtualFile;
import com.intellij.openapi.fileTypes.FileTypeManager;

public class ExampleViewerService {
    private static final ExampleViewerService INSTANCE = new ExampleViewerService();

    public static ExampleViewerService getInstance() {
        return INSTANCE;
    }

    public void showExample(Project project, ExampleCategory category) {
        String content = ExampleContentProvider.getContent(category);
        String fileName = category.getFileName();
        
        LightVirtualFile virtualFile = new LightVirtualFile(fileName, 
            FileTypeManager.getInstance().getFileTypeByExtension("cs"), content);
        virtualFile.setWritable(false);
        
        FileEditorManager.getInstance(project).openFile(virtualFile, true);
    }
}