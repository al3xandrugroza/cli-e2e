package com.clie2e.examples;

public enum ExampleCategory {
    ANALYZE("Analyze", "analyze_examples.cs"),
    DEPLOY("Deploy", "deploy_examples.cs"),
    MANAGE_ASSETS("Manage Assets", "manage_assets_examples.cs"),
    PACK("Pack", "pack_examples.cs"),
    RUN_JOB("Run Job", "run_job_examples.cs"),
    RUN_TESTS("Run Tests", "run_tests_examples.cs");

    private final String displayName;
    private final String fileName;

    ExampleCategory(String displayName, String fileName) {
        this.displayName = displayName;
        this.fileName = fileName;
    }

    public String getDisplayName() {
        return displayName;
    }

    public String getFileName() {
        return fileName;
    }
}